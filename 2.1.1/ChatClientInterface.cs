using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IPROG.Uppgifter.uppg2_1_1
{
    /// <summary>
    /// CLI Wrapper for chat client to avoid message collisions when writing and receiving messages at the same time.
    /// </summary>
    internal class ChatClientInterface
    {
        private ChatClient _client;
        private readonly List<char> _inputBuffer;

        /// <summary>
        /// Creates a new <see cref="ChatClientInterface"/>.
        /// </summary>
        public ChatClientInterface()
        {
            _inputBuffer = new List<char>();
        }

        /// <summary>
        /// Attaches the interface to the client provided.
        /// </summary>
        /// <param name="client">Chat client to connect to the CLI</param>
        public void AttachInput(ChatClient client)
        {
            _client = client;
            _client.OnMessage += (_, ev) => HandleIncomingMessage(ev.MessageText);
        }

        /// <summary>
        /// Starts listening for input.
        /// </summary>
        public void StartAcceptInput()
        {
            while (true)
            {
                Console.Write("> ");

                ReadLine();

                var msg = new string(_inputBuffer.ToArray());

                Task.Run(async () => await _client.SendMessageAsync(msg));
                _inputBuffer.Clear();
            }
        }

        /// <summary>
        /// Reads input until an enter stroke is detected.
        /// </summary>
        private void ReadLine()
        {
            while (true)
            {
                // Read key stroke
                var lastKey = Console.ReadKey();

                switch (lastKey.Key)
                {
                    // Stop reading if enter key is pressed
                    case ConsoleKey.Enter:
                        return;
                    // Delete last character if key is backspace
                    case ConsoleKey.Backspace when _inputBuffer.Count > 0:
                        _inputBuffer.RemoveAt(_inputBuffer.Count - 1);
                        RebuildLine();
                        continue;
                    default:
                        _inputBuffer.Add(lastKey.KeyChar);
                        break;
                }
            }
        }

        /// <summary>
        /// Clears all input from the current line
        /// </summary>
        private static void ClearCurrentLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop);

            for (var i = 1; i < Console.WindowWidth; i++)
            {
                Console.Write(" ");
            }

            Console.SetCursorPosition(0, Console.CursorTop);
        }

        /// <summary>
        /// Prints incoming message by printing it and avoiding conflicts with input.
        /// </summary>
        /// <param name="text">Message </param>
        private void HandleIncomingMessage(string text)
        {
            ClearCurrentLine();
            Console.WriteLine(text);
            RebuildLine();
        }

        /// <summary>
        /// Rebuilds the CLI input line from the input buffer by clearing it and writing the contents of the buffer.
        /// </summary>
        private void RebuildLine()
        {
            ClearCurrentLine();
            Console.Write($"> {new string(_inputBuffer.ToArray())}");
        }
    }
}
