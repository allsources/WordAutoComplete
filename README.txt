
Search popular words by prefix using prefix tree.

*) WordAutoComplete.exe [ -trie | -dictionary | -hashset ]

Standalone console application.

Commands:

  <prefix>
or
  -match <prefix>

  Search words by entered <prefix>. Command without <prefix> process all prefixes from the WordPrefixCollection.txt file.

  -add <word> <count>

  Add a new word to collection.

  -get <word>

  Get a word from collection.

  -del <word>

  Remove a word from collection.

*) WordAutoCompleteClient.exe

Allow to send <prefix> to the WordAutoCompleteServer and receive from it popular words.

Host name and number of a Port for connection contains in the config file.

*) WordAutoCompleteServer.exe

Receives <prefix> from the WordAutoCompleteClient  and then performs a search for popular words using the prefix tree.

Number of a Port for listening contains in the config file.
