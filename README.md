# Monda
Performant parser-combinator framework targeting .NET Standard 2.0.
Monda is built to take advantage of the recent performance-driven APIs released by the .NET team.

## Usage
See the [API docs](API.md) for examples

### Future goals
Currently the API surface is built around a `ReadOnlySpan<T>` as the source of data to be parsed. 
This has so far been adequate but comes with the requirement that all data to parsed must be available in contiguous memory. 
I would like to investigate the possibility of allowing the parser to handle `ReadOnlySequence<T>` as well, to allow for parsing buffered and non-contiguous data; 
similar to how [`Utf8JsonReader`](https://docs.microsoft.com/en-us/dotnet/api/system.text.json.utf8jsonreader?view=netcore-3.1) is implemented.