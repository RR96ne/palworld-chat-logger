#include <Mod/CppUserModBase.hpp>
#include <DynamicOutput/DynamicOutput.hpp>
#include <Unreal/UObjectGlobals.hpp>
#include <Unreal/UObject.hpp>
//#include <Unreal/FString.hpp>
#include <Unreal/UClass.hpp>
#include <Unreal/UFunction.hpp>
//#include <Unreal/ReflectedFunction.hpp>
//#include <Unreal/GameplayStatics.hpp>
#include <Unreal/Hooks.hpp>
#include <Unreal/World.hpp>
#include <Script.hpp>
#include <windows.h> 
#include <stdio.h>
#include <conio.h>
#include <tchar.h>
#include <locale.h>

#define BUFSIZE 512
LPCTSTR PIPENAME = TEXT("\\\\.\\pipe\\palworld_chat_logger");

using namespace RC;
using namespace RC::Unreal;

enum class EPalChatCategory {
	None = 0,
	Global = 1,
	Guild = 2,
	Say = 3,
	EPalChatCategory_MAX = 4,
};

struct FPalChatMessage
{
	EPalChatCategory Category;                                                        // 0x0000 (size: 0x1)
	FString Sender;                                                                   // 0x0008 (size: 0x10)
	FGuid SenderPlayerUId;                                                            // 0x0018 (size: 0x10)
	FString Message;                                                                  // 0x0028 (size: 0x10)
	FGuid ReceiverPlayerUId;                                                          // 0x0038 (size: 0x10)

};

class ChatLoggerMod : public RC::CppUserModBase
{
public:
    ChatLoggerMod() : CppUserModBase()
    {
		setlocale(LC_ALL, "Japanese");

        ModName = STR("ChatLoggerMod");
        ModVersion = STR("1.0.0");
        ModDescription = STR("This is for outputting in-game chat to a named pipe.");
        ModAuthors = STR("RR96ne");

        // Do not change this unless you want to target a UE4SS version
        // other than the one you're currently building with somehow.
        //ModIntendedSDKVersion = STR("2.6");

		Hook::RegisterProcessEventPostCallback(&process_event_hook);
    }

    ~ChatLoggerMod() override
    {
    }

    auto on_update() -> void override
    {
    }
    auto on_unreal_init() -> void override
    {
    }

	static auto process_event_hook(Unreal::UObject* Context, Unreal::UFunction* Function, void* Parms) -> void
	{
		if (!(Context && Context->GetClassPrivate() && Function)) return;

		static FName func_name = FName(STR("OnReceivedChat"), FNAME_Add);
		static FName class_name = FName(STR("WBP_Ingame_Chat_C"), FNAME_Add);

		if (Function->GetNamePrivate() == func_name && Context->GetClassPrivate()->GetNamePrivate() == class_name)
		{
			const FPalChatMessage* message = static_cast<FPalChatMessage*>(Parms);
			const TCHAR* message_str = message->Message.GetCharTArray().GetData();
			const TCHAR* sender_str = message->Sender.GetCharTArray().GetData();
			EPalChatCategory category = message->Category;
			int category_int = (int)(category) & 7; // For some reason this program can't get the correct values.

			Output::send<LogLevel::Verbose>(STR("{}{}:{}\n"), category_int, sender_str, message_str);

			TCHAR buffer[BUFSIZE];
			_stprintf(buffer, TEXT("%i[%s]%s"), category_int, sender_str, message_str);

			send_to_pipe(buffer);
		}
	}

	static int send_to_pipe(TCHAR* arg)
	{
		HANDLE hPipe;
		LPCTSTR lpvMessage = arg;
		BOOL   fSuccess = FALSE;
		DWORD  cbToWrite, cbWritten;

		while (1)
		{
			hPipe = CreateFile(
				PIPENAME,   // pipe name 
				GENERIC_WRITE,
				0,              // no sharing 
				NULL,           // default security attributes
				OPEN_EXISTING,  // opens existing pipe 
				0,              // default attributes 
				NULL);          // no template file 

			// Break if the pipe handle is valid. 
			if (hPipe != INVALID_HANDLE_VALUE)
				break;

			// Exit if an error other than ERROR_PIPE_BUSY occurs. 
			if (GetLastError() != ERROR_PIPE_BUSY)
			{
				Output::send<LogLevel::Verbose>(STR("Could not open pipe.\n"));
				return -1;
			}

			// All pipe instances are busy, so wait for a second. 
			if (!WaitNamedPipe(PIPENAME, 1000))
			{
				Output::send<LogLevel::Verbose>(STR("Could not open pipe: 1 second wait timed out.\n"));
				return -1;
			}
		}

		cbToWrite = (lstrlen(lpvMessage) + 1) * sizeof(TCHAR);

		fSuccess = WriteFile(
			hPipe,                  // pipe handle 
			lpvMessage,             // message 
			cbToWrite,              // message length 
			&cbWritten,             // bytes written 
			NULL);                  // not overlapped 

		if (!fSuccess)
		{
			Output::send<LogLevel::Verbose>(STR("WriteFile to pipe failed.\n"));
			return -1;
		}

		CloseHandle(hPipe);
		return 0;
	}
};

#define CHAT_LOGGER_MOD __declspec(dllexport)
extern "C"
{
    CHAT_LOGGER_MOD RC::CppUserModBase* start_mod()
    {
        return new ChatLoggerMod();
    }

    CHAT_LOGGER_MOD void uninstall_mod(RC::CppUserModBase* mod)
    {
        delete mod;
    }
}