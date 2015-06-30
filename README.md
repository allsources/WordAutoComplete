
Search popular words by prefix using prefix tree. [Download](http://1drv.ms/1eg6W7s) source code and executable files.

Three console apps will be available in the Build folder after building a project.

* WordAutoComplete.exe [ -trie | -dictionary | -hashset ]

Standalone console application.

Commands:

  `*prefix*`
  or
  `-match *prefix*`

  Search words by specified `*prefix*`. Command without `*prefix*` processes all prefixes from the WordPrefixCollection.txt file.

  `-add *word* *count*`

  Add a new word to collection.

  `-get *word*`

  Get a word from collection.

  `-del *word*`

  Remove a word from collection.

* WordAutoCompleteClient.exe

Allow to send `*prefix*` to the WordAutoCompleteServer and receive back popular words.

*Host* name and *Port* number for connection are specified in the config file.

* WordAutoCompleteServer.exe

Receives `*prefix*` from the WordAutoCompleteClient and performs a search for popular words using the prefix tree.

*Port* number to listen on is specified in the config file.
