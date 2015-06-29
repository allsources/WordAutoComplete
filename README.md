
Search popular words by prefix using prefix tree.

*) WordAutoComplete.exe [ -trie | -dictionary | -hashset ]

Standalone console application.

Commands:

  *prefix*
or
  -match <prefix>

  Search words by specified <prefix>. Command without <prefix> processes all prefixes from the WordPrefixCollection.txt file.

  -add <word> <count>

  Add a new word to collection.

  -get <word>

  Get a word from collection.

  -del <word>

  Remove a word from collection.

*) WordAutoCompleteClient.exe

Allow to send <prefix> to the WordAutoCompleteServer and receive popular words.

Host name and port number for connection are specified in the config file.

*) WordAutoCompleteServer.exe

Receives <prefix> from the WordAutoCompleteClient and performs a search for popular words using the prefix tree.

Port number to listen on is specified in the config file.
