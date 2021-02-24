<a name='assembly'></a>
# Monda

## Contents

- [ParseFunction\`2](#T-Monda-ParseFunction`2 'Monda.ParseFunction`2')
- [ParseResult](#T-Monda-ParseResult 'Monda.ParseResult')
  - [Fail\`\`1()](#M-Monda-ParseResult-Fail``1 'Monda.ParseResult.Fail``1')
  - [Fail\`\`1(value)](#M-Monda-ParseResult-Fail``1-``0@- 'Monda.ParseResult.Fail``1(``0@)')
  - [Success\`\`1(value,start,length)](#M-Monda-ParseResult-Success``1-``0@,System-Int32,System-Int32- 'Monda.ParseResult.Success``1(``0@,System.Int32,System.Int32)')
- [ParseResult\`1](#T-Monda-ParseResult`1 'Monda.ParseResult`1')
  - [Length](#P-Monda-ParseResult`1-Length 'Monda.ParseResult`1.Length')
  - [Start](#P-Monda-ParseResult`1-Start 'Monda.ParseResult`1.Start')
  - [Success](#P-Monda-ParseResult`1-Success 'Monda.ParseResult`1.Success')
  - [Value](#P-Monda-ParseResult`1-Value 'Monda.ParseResult`1.Value')
- [Parser](#T-Monda-Parser 'Monda.Parser')
  - [IsAny\`\`1(values)](#M-Monda-Parser-IsAny``1-System-Collections-Generic-IReadOnlyList{``0}- 'Monda.Parser.IsAny``1(System.Collections.Generic.IReadOnlyList{``0})')
  - [IsAny\`\`1(values,comparer)](#M-Monda-Parser-IsAny``1-System-Collections-Generic-IReadOnlyList{``0},System-Collections-Generic-IEqualityComparer{``0}- 'Monda.Parser.IsAny``1(System.Collections.Generic.IReadOnlyList{``0},System.Collections.Generic.IEqualityComparer{``0})')
  - [IsNotAny\`\`1(values)](#M-Monda-Parser-IsNotAny``1-System-Collections-Generic-IReadOnlyList{``0}- 'Monda.Parser.IsNotAny``1(System.Collections.Generic.IReadOnlyList{``0})')
  - [IsNotAny\`\`1(values,comparer)](#M-Monda-Parser-IsNotAny``1-System-Collections-Generic-IReadOnlyList{``0},System-Collections-Generic-IEqualityComparer{``0}- 'Monda.Parser.IsNotAny``1(System.Collections.Generic.IReadOnlyList{``0},System.Collections.Generic.IEqualityComparer{``0})')
  - [IsNot\`\`1(value)](#M-Monda-Parser-IsNot``1-``0- 'Monda.Parser.IsNot``1(``0)')
  - [IsNot\`\`1(value,comparer)](#M-Monda-Parser-IsNot``1-``0,System-Collections-Generic-IEqualityComparer{``0}- 'Monda.Parser.IsNot``1(``0,System.Collections.Generic.IEqualityComparer{``0})')
  - [IsNot\`\`1(predicate)](#M-Monda-Parser-IsNot``1-System-Func{``0,System-Boolean}- 'Monda.Parser.IsNot``1(System.Func{``0,System.Boolean})')
  - [IsSequence\`\`1(value)](#M-Monda-Parser-IsSequence``1-System-Collections-Generic-IReadOnlyList{``0}- 'Monda.Parser.IsSequence``1(System.Collections.Generic.IReadOnlyList{``0})')
  - [IsSequence\`\`1(value,comparer)](#M-Monda-Parser-IsSequence``1-System-Collections-Generic-IReadOnlyList{``0},System-Collections-Generic-IEqualityComparer{``0}- 'Monda.Parser.IsSequence``1(System.Collections.Generic.IReadOnlyList{``0},System.Collections.Generic.IEqualityComparer{``0})')
  - [Is\`\`1(value)](#M-Monda-Parser-Is``1-``0- 'Monda.Parser.Is``1(``0)')
  - [Is\`\`1(value,comparer)](#M-Monda-Parser-Is``1-``0,System-Collections-Generic-IEqualityComparer{``0}- 'Monda.Parser.Is``1(``0,System.Collections.Generic.IEqualityComparer{``0})')
  - [Is\`\`1(predicate)](#M-Monda-Parser-Is``1-System-Func{``0,System-Boolean}- 'Monda.Parser.Is``1(System.Func{``0,System.Boolean})')
  - [TakeUntil\`\`2(next,min)](#M-Monda-Parser-TakeUntil``2-Monda-Parser{``0,``1},System-Int32- 'Monda.Parser.TakeUntil``2(Monda.Parser{``0,``1},System.Int32)')
  - [TakeWhile\`\`1(predicate,min,max)](#M-Monda-Parser-TakeWhile``1-System-Func{``0,System-Boolean},System-Int32,System-Nullable{System-Int32}- 'Monda.Parser.TakeWhile``1(System.Func{``0,System.Boolean},System.Int32,System.Nullable{System.Int32})')
  - [TakeWhile\`\`1(predicate,min,max)](#M-Monda-Parser-TakeWhile``1-Monda-ParserPredicate{``0},System-Int32,System-Nullable{System-Int32}- 'Monda.Parser.TakeWhile``1(Monda.ParserPredicate{``0},System.Int32,System.Nullable{System.Int32})')
- [ParserExtensions](#T-Monda-ParserExtensions 'Monda.ParserExtensions')
  - [Between\`\`3(self,other)](#M-Monda-ParserExtensions-Between``3-Monda-Parser{``0,``1},Monda-Parser{``0,``2}- 'Monda.ParserExtensions.Between``3(Monda.Parser{``0,``1},Monda.Parser{``0,``2})')
  - [Between\`\`4(self,left,right)](#M-Monda-ParserExtensions-Between``4-Monda-Parser{``0,``1},Monda-Parser{``0,``2},Monda-Parser{``0,``3}- 'Monda.ParserExtensions.Between``4(Monda.Parser{``0,``1},Monda.Parser{``0,``2},Monda.Parser{``0,``3})')
  - [FollowedBy\`\`3(self,next)](#M-Monda-ParserExtensions-FollowedBy``3-Monda-Parser{``0,``1},Monda-Parser{``0,``2}- 'Monda.ParserExtensions.FollowedBy``3(Monda.Parser{``0,``1},Monda.Parser{``0,``2})')
  - [ManyUntil\`\`3(self,next,min)](#M-Monda-ParserExtensions-ManyUntil``3-Monda-Parser{``0,``1},Monda-Parser{``0,``2},System-Int32- 'Monda.ParserExtensions.ManyUntil``3(Monda.Parser{``0,``1},Monda.Parser{``0,``2},System.Int32)')
  - [Many\`\`2(self,min,max)](#M-Monda-ParserExtensions-Many``2-Monda-Parser{``0,``1},System-Int32,System-Nullable{System-Int32}- 'Monda.ParserExtensions.Many``2(Monda.Parser{``0,``1},System.Int32,System.Nullable{System.Int32})')
  - [Map\`\`3(self,map)](#M-Monda-ParserExtensions-Map``3-Monda-Parser{``0,``1},Monda-MapFunction{``0,``1,``2}- 'Monda.ParserExtensions.Map``3(Monda.Parser{``0,``1},Monda.MapFunction{``0,``1,``2})')
  - [Optional\`\`2(self,defaultValue)](#M-Monda-ParserExtensions-Optional``2-Monda-Parser{``0,``1},``1- 'Monda.ParserExtensions.Optional``2(Monda.Parser{``0,``1},``1)')
  - [Or\`\`2(self,other)](#M-Monda-ParserExtensions-Or``2-Monda-Parser{``0,``1},Monda-Parser{``0,``1}- 'Monda.ParserExtensions.Or``2(Monda.Parser{``0,``1},Monda.Parser{``0,``1})')
  - [PrecededBy\`\`3(self,previous)](#M-Monda-ParserExtensions-PrecededBy``3-Monda-Parser{``0,``1},Monda-Parser{``0,``2}- 'Monda.ParserExtensions.PrecededBy``3(Monda.Parser{``0,``1},Monda.Parser{``0,``2})')
  - [Repeat\`\`2(self,count)](#M-Monda-ParserExtensions-Repeat``2-Monda-Parser{``0,``1},System-Int32- 'Monda.ParserExtensions.Repeat``2(Monda.Parser{``0,``1},System.Int32)')
  - [SkipMany\`\`2(self,min,max)](#M-Monda-ParserExtensions-SkipMany``2-Monda-Parser{``0,``1},System-Int32,System-Nullable{System-Int32}- 'Monda.ParserExtensions.SkipMany``2(Monda.Parser{``0,``1},System.Int32,System.Nullable{System.Int32})')
  - [SkipUntil\`\`3(self,next,min)](#M-Monda-ParserExtensions-SkipUntil``3-Monda-Parser{``0,``1},Monda-Parser{``0,``2},System-Int32- 'Monda.ParserExtensions.SkipUntil``3(Monda.Parser{``0,``1},Monda.Parser{``0,``2},System.Int32)')
  - [Skip\`\`2(self,count)](#M-Monda-ParserExtensions-Skip``2-Monda-Parser{``0,``1},System-Int32- 'Monda.ParserExtensions.Skip``2(Monda.Parser{``0,``1},System.Int32)')
  - [Then\`\`3(self,next)](#M-Monda-ParserExtensions-Then``3-Monda-Parser{``0,``1},Monda-Parser{``0,``2}- 'Monda.ParserExtensions.Then``3(Monda.Parser{``0,``1},Monda.Parser{``0,``2})')
  - [TryMap\`\`3(self,tryMap)](#M-Monda-ParserExtensions-TryMap``3-Monda-Parser{``0,``1},Monda-TryMapFunction{``0,``1,``2}- 'Monda.ParserExtensions.TryMap``3(Monda.Parser{``0,``1},Monda.TryMapFunction{``0,``1,``2})')
- [ParserPredicate\`1](#T-Monda-ParserPredicate`1 'Monda.ParserPredicate`1')
- [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2')
  - [Name](#P-Monda-Parser`2-Name 'Monda.Parser`2.Name')
- [Range](#T-Monda-Range 'Monda.Range')

<a name='T-Monda-ParseFunction`2'></a>
## ParseFunction\`2 `type`

##### Namespace

Monda

##### Summary

Delegate that attempts to parse a result from an input data set, beginning at a specific point

##### Returns

A [ParseResult\`1](#T-Monda-ParseResult`1 'Monda.ParseResult`1') indicating the success or failure of the parse attempt

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| data | [T:Monda.ParseFunction\`2](#T-T-Monda-ParseFunction`2 'T:Monda.ParseFunction`2') | Input data to parse |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |
| TResult | The result type of the parser |

<a name='T-Monda-ParseResult'></a>
## ParseResult `type`

##### Namespace

Monda

##### Summary

Generic helper functions for constructing [ParseResult\`1](#T-Monda-ParseResult`1 'Monda.ParseResult`1') instances

<a name='M-Monda-ParseResult-Fail``1'></a>
### Fail\`\`1() `method`

##### Summary

Create a failure result with the default value for `T`

##### Returns

A failure [ParseResult\`1](#T-Monda-ParseResult`1 'Monda.ParseResult`1') with it's value set to default(`T`)

##### Parameters

This method has no parameters.

##### Generic Types

| Name | Description |
| ---- | ----------- |
| T | The underlying value type |

<a name='M-Monda-ParseResult-Fail``1-``0@-'></a>
### Fail\`\`1(value) `method`

##### Summary

Create a failure result with a specified value

##### Returns

A failure [ParseResult\`1](#T-Monda-ParseResult`1 'Monda.ParseResult`1') with it's value set to `value`

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| value | [\`\`0@](#T-``0@ '``0@') | The value of the result |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| T | The underlying value type |

<a name='M-Monda-ParseResult-Success``1-``0@,System-Int32,System-Int32-'></a>
### Success\`\`1(value,start,length) `method`

##### Summary

Create a success result

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| value | [\`\`0@](#T-``0@ '``0@') | The value of the result |
| start | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The position at which the result started |
| length | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The length of data used to parse `value` |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| T | The underlying value type |

<a name='T-Monda-ParseResult`1'></a>
## ParseResult\`1 `type`

##### Namespace

Monda

##### Summary

Represents the result of a parsing operation

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TValue | Underlying value type |

##### See Also

- [Monda.ParseResult.Success\`\`1](#M-Monda-ParseResult-Success``1-``0@,System-Int32,System-Int32- 'Monda.ParseResult.Success``1(``0@,System.Int32,System.Int32)')
- [Monda.ParseResult.Fail\`\`1](#M-Monda-ParseResult-Fail``1 'Monda.ParseResult.Fail``1')
- [Monda.ParseResult.Fail\`\`1](#M-Monda-ParseResult-Fail``1-``0@- 'Monda.ParseResult.Fail``1(``0@)')

<a name='P-Monda-ParseResult`1-Length'></a>
### Length `property`

##### Summary

The length of data in the data source used to construct this result

<a name='P-Monda-ParseResult`1-Start'></a>
### Start `property`

##### Summary

The position in the data source where this result started

<a name='P-Monda-ParseResult`1-Success'></a>
### Success `property`

##### Summary

Indicates if this result was successful

<a name='P-Monda-ParseResult`1-Value'></a>
### Value `property`

##### Summary

The underlying result value

<a name='T-Monda-Parser'></a>
## Parser `type`

##### Namespace

Monda

##### Summary

Static Parser helper methods

<a name='M-Monda-Parser-IsAny``1-System-Collections-Generic-IReadOnlyList{``0}-'></a>
### IsAny\`\`1(values) `method`

##### Summary

Creates a [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that yields a single `TSource` item if it exists in `values`, using the default comparer for `TSource` to compare items

##### Returns

A [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that will succeed if the `TSource` item at the parser's current position exists in `values`

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| values | [System.Collections.Generic.IReadOnlyList{\`\`0}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IReadOnlyList 'System.Collections.Generic.IReadOnlyList{``0}') | Item to compare |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `values` is null |

<a name='M-Monda-Parser-IsAny``1-System-Collections-Generic-IReadOnlyList{``0},System-Collections-Generic-IEqualityComparer{``0}-'></a>
### IsAny\`\`1(values,comparer) `method`

##### Summary

Creates a [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that yields a single `TSource` item if it exists in `values`, using `comparer` to compare values

##### Returns

A [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that will succeed if the `TSource` item at the parser's current position exists in `values`

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| values | [System.Collections.Generic.IReadOnlyList{\`\`0}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IReadOnlyList 'System.Collections.Generic.IReadOnlyList{``0}') | Item to compare |
| comparer | [System.Collections.Generic.IEqualityComparer{\`\`0}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IEqualityComparer 'System.Collections.Generic.IEqualityComparer{``0}') | Comparer to use to determine equality between `TSource` items |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentException 'System.ArgumentException') | `values` is empty |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `values` or `comparer` is null |

<a name='M-Monda-Parser-IsNotAny``1-System-Collections-Generic-IReadOnlyList{``0}-'></a>
### IsNotAny\`\`1(values) `method`

##### Summary

Creates a [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that yields a single `TSource` item if it does not exist in `values`, using the default comparer for `TSource` to compare items

##### Returns

A [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that will succeed if the `TSource` item at the parser's current position does not exist in `values`

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| values | [System.Collections.Generic.IReadOnlyList{\`\`0}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IReadOnlyList 'System.Collections.Generic.IReadOnlyList{``0}') | Item to compare |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `values` is null |

<a name='M-Monda-Parser-IsNotAny``1-System-Collections-Generic-IReadOnlyList{``0},System-Collections-Generic-IEqualityComparer{``0}-'></a>
### IsNotAny\`\`1(values,comparer) `method`

##### Summary

Creates a [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that yields a single `TSource` item if it does not exist in `values`, using `comparer` to compare values

##### Returns

A [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that will succeed if the `TSource` item at the parser's current position does not exist in `values`

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| values | [System.Collections.Generic.IReadOnlyList{\`\`0}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IReadOnlyList 'System.Collections.Generic.IReadOnlyList{``0}') | Item to compare |
| comparer | [System.Collections.Generic.IEqualityComparer{\`\`0}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IEqualityComparer 'System.Collections.Generic.IEqualityComparer{``0}') | Comparer to use to determine equality between `TSource` items |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentException 'System.ArgumentException') | `values` is empty |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `values` or `comparer` is null |

<a name='M-Monda-Parser-IsNot``1-``0-'></a>
### IsNot\`\`1(value) `method`

##### Summary

Creates a [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that yields a single `TSource` item if it does not equal `value`, using the default comparer for `TSource` to compare items

##### Returns

A parser that will succeed if the `TSource` item at the parser's current position does not equal `value`

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| value | [\`\`0](#T-``0 '``0') | Item to match |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |

<a name='M-Monda-Parser-IsNot``1-``0,System-Collections-Generic-IEqualityComparer{``0}-'></a>
### IsNot\`\`1(value,comparer) `method`

##### Summary

Creates a [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that yields a single `TSource` item if it does not equal `value`, using `comparer` to compare items

##### Returns

A parser that will succeed if the `TSource` item at the parser's current position does not equal `value`

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| value | [\`\`0](#T-``0 '``0') | Item to match |
| comparer | [System.Collections.Generic.IEqualityComparer{\`\`0}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IEqualityComparer 'System.Collections.Generic.IEqualityComparer{``0}') | Comparer used to determine equality between `TSource` items |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `comparer` is null |

<a name='M-Monda-Parser-IsNot``1-System-Func{``0,System-Boolean}-'></a>
### IsNot\`\`1(predicate) `method`

##### Summary

Creates a [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that yields a single `TSource` item if it does not pass `predicate`

##### Returns

A parser that will succeed if the `TSource` item at the parser's current position does not pass `predicate`

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| predicate | [System.Func{\`\`0,System.Boolean}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Func 'System.Func{``0,System.Boolean}') | Predicate that is invoked against the next `TSource` item |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `predicate` is null |

<a name='M-Monda-Parser-IsSequence``1-System-Collections-Generic-IReadOnlyList{``0}-'></a>
### IsSequence\`\`1(value) `method`

##### Summary

Creates a [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that yields a [IReadOnlyList\`1](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IReadOnlyList`1 'System.Collections.Generic.IReadOnlyList`1') if the sequence of items at the current position equals `value`, using the default comparer for `TSource` to compare items

##### Returns

A [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that will succeed if the sequence of `TSource` items at the parser's current position equals `value`

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| value | [System.Collections.Generic.IReadOnlyList{\`\`0}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IReadOnlyList 'System.Collections.Generic.IReadOnlyList{``0}') | [ReadOnlyMemory\`1](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ReadOnlyMemory`1 'System.ReadOnlyMemory`1') containing the sequence of `TSource` items to compare |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |

<a name='M-Monda-Parser-IsSequence``1-System-Collections-Generic-IReadOnlyList{``0},System-Collections-Generic-IEqualityComparer{``0}-'></a>
### IsSequence\`\`1(value,comparer) `method`

##### Summary

Creates a [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that yields a [IReadOnlyList\`1](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IReadOnlyList`1 'System.Collections.Generic.IReadOnlyList`1') if the sequence of items at the current position equals `value`, using `comparer` to compare items

##### Returns

A [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that will succeed if the sequence of `TSource` items at the parser's current position equals `value`

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| value | [System.Collections.Generic.IReadOnlyList{\`\`0}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IReadOnlyList 'System.Collections.Generic.IReadOnlyList{``0}') | [ReadOnlyMemory\`1](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ReadOnlyMemory`1 'System.ReadOnlyMemory`1') containing the sequence of `TSource` items to compare |
| comparer | [System.Collections.Generic.IEqualityComparer{\`\`0}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IEqualityComparer 'System.Collections.Generic.IEqualityComparer{``0}') | Comparer used to determine equality between `TSource` items |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentException 'System.ArgumentException') | `value` is empty |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `value` or `comparer` is null |

<a name='M-Monda-Parser-Is``1-``0-'></a>
### Is\`\`1(value) `method`

##### Summary

Creates a [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that yields a single `TSource` item if it equals `value`, using the default comparer for `TSource` to compare items

##### Returns

A parser that will succeed if the `TSource` item at the parser's current position equals `value`

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| value | [\`\`0](#T-``0 '``0') | Item to match |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |

<a name='M-Monda-Parser-Is``1-``0,System-Collections-Generic-IEqualityComparer{``0}-'></a>
### Is\`\`1(value,comparer) `method`

##### Summary

Creates a [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that yields a single `TSource` item if it equals `value`, using the `comparer` to compare items.

##### Returns

A [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that will succeed if the `TSource` item at the parser's current position equals `value`

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| value | [\`\`0](#T-``0 '``0') | Item to compare |
| comparer | [System.Collections.Generic.IEqualityComparer{\`\`0}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IEqualityComparer 'System.Collections.Generic.IEqualityComparer{``0}') | Comparer used to determine equality between `TSource` items |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |

<a name='M-Monda-Parser-Is``1-System-Func{``0,System-Boolean}-'></a>
### Is\`\`1(predicate) `method`

##### Summary

Creates a [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that yields a single `TSource` item if it passes `predicate`

##### Returns

A parser that will succeed if the `TSource` item at the parser's current position passes `predicate`

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| predicate | [System.Func{\`\`0,System.Boolean}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Func 'System.Func{``0,System.Boolean}') | Predicate that is invoked against the next `TSource` item |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `predicate` is null |

<a name='M-Monda-Parser-TakeUntil``2-Monda-Parser{``0,``1},System-Int32-'></a>
### TakeUntil\`\`2(next,min) `method`

##### Summary

Creates a [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that yields the range of `TSource` items from the current parser until `next` succeeds

##### Returns

A [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that yields a tuple containing the [Range](#T-Monda-Range 'Monda.Range') that was matched and the result of `next`

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| next | [Monda.Parser{\`\`0,\`\`1}](#T-Monda-Parser{``0,``1} 'Monda.Parser{``0,``1}') | A [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that will succeed at the end of the range |
| min | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Minimum number of matches required (inclusive) |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |
| TNext | The return type of the next parser |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `next` is null |
| [System.ArgumentOutOfRangeException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentOutOfRangeException 'System.ArgumentOutOfRangeException') | `min` is less than 0 |

<a name='M-Monda-Parser-TakeWhile``1-System-Func{``0,System-Boolean},System-Int32,System-Nullable{System-Int32}-'></a>
### TakeWhile\`\`1(predicate,min,max) `method`

##### Summary

Creates a [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that yields the range of `TSource` items from the current parser position matching `predicate`

##### Returns

A [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that yields the [Range](#T-Monda-Range 'Monda.Range') of data that was matched

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| predicate | [System.Func{\`\`0,System.Boolean}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Func 'System.Func{``0,System.Boolean}') | Predicate that is invoked for each `TSource` item |
| min | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Minimum number of matches required (inclusive) |
| max | [System.Nullable{System.Int32}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.Int32}') | Maximum number of matches allowed (inclusive) |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `predicate` is null |
| [System.ArgumentOutOfRangeException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentOutOfRangeException 'System.ArgumentOutOfRangeException') | `min` is less than 0 or `max` is less than `min` |

<a name='M-Monda-Parser-TakeWhile``1-Monda-ParserPredicate{``0},System-Int32,System-Nullable{System-Int32}-'></a>
### TakeWhile\`\`1(predicate,min,max) `method`

##### Summary

Creates a [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that yields the range of `TSource` items from the current parser position matching `predicate`

##### Returns

A [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that yields the [Range](#T-Monda-Range 'Monda.Range') of data that was matched

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| predicate | [Monda.ParserPredicate{\`\`0}](#T-Monda-ParserPredicate{``0} 'Monda.ParserPredicate{``0}') | Predicate that is invoked for each `TSource` item |
| min | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Minimum number of matches required (inclusive) |
| max | [System.Nullable{System.Int32}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.Int32}') | Maximum number of matches allowed (inclusive) |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `predicate` is null |
| [System.ArgumentOutOfRangeException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentOutOfRangeException 'System.ArgumentOutOfRangeException') | `min` is less than 0 or `max` is less than `min` |

##### Remarks

The [ReadOnlySpan\`1](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ReadOnlySpan`1 'System.ReadOnlySpan`1') passed to `predicate` is a slice starting at the parser's intial position and the index will increase from 0

<a name='T-Monda-ParserExtensions'></a>
## ParserExtensions `type`

##### Namespace

Monda

<a name='M-Monda-ParserExtensions-Between``3-Monda-Parser{``0,``1},Monda-Parser{``0,``2}-'></a>
### Between\`\`3(self,other) `method`

##### Summary

Creates a new parser that ensures that the current parser is surrounded by `other`

##### Returns

A new [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that returns the result of the current parser if it is found between `other`

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| self | [Monda.Parser{\`\`0,\`\`1}](#T-Monda-Parser{``0,``1} 'Monda.Parser{``0,``1}') | The current parser |
| other | [Monda.Parser{\`\`0,\`\`2}](#T-Monda-Parser{``0,``2} 'Monda.Parser{``0,``2}') | The parser to match before and after the current parser |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |
| TResult | The result type of `self` |
| TOther | The result type of `other` |

<a name='M-Monda-ParserExtensions-Between``4-Monda-Parser{``0,``1},Monda-Parser{``0,``2},Monda-Parser{``0,``3}-'></a>
### Between\`\`4(self,left,right) `method`

##### Summary

Creates a new parser that ensures that the current parser is between `left` and `right`

##### Returns

A new [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that returns the result of the current parser if it is found between `left` and `right`

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| self | [Monda.Parser{\`\`0,\`\`1}](#T-Monda-Parser{``0,``1} 'Monda.Parser{``0,``1}') | The current parser |
| left | [Monda.Parser{\`\`0,\`\`2}](#T-Monda-Parser{``0,``2} 'Monda.Parser{``0,``2}') | The first parser to be executed |
| right | [Monda.Parser{\`\`0,\`\`3}](#T-Monda-Parser{``0,``3} 'Monda.Parser{``0,``3}') | The last parser to execute |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |
| TResult | The result type of `self` |
| TLeft | The result type of `left` |
| TRight | The result type of `right` |

<a name='M-Monda-ParserExtensions-FollowedBy``3-Monda-Parser{``0,``1},Monda-Parser{``0,``2}-'></a>
### FollowedBy\`\`3(self,next) `method`

##### Summary

Create a new parser that ensures that the current parser is followed by `next`

##### Returns

A new [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that executes `self` and `next` in that order and returns the `TResult` from `self`

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| self | [Monda.Parser{\`\`0,\`\`1}](#T-Monda-Parser{``0,``1} 'Monda.Parser{``0,``1}') | The current parser |
| next | [Monda.Parser{\`\`0,\`\`2}](#T-Monda-Parser{``0,``2} 'Monda.Parser{``0,``2}') | The parser to execute after the current parser, its result will be discarded |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |
| TResult | The result type of `self` |
| TNext | The result type of `next` |

##### Remarks

Although the parsing position will be advanced accordingly, the result of `next` will be discarded. Use [Then\`\`3](#M-Monda-ParserExtensions-Then``3-Monda-Parser{``0,``1},Monda-Parser{``0,``2}- 'Monda.ParserExtensions.Then``3(Monda.Parser{``0,``1},Monda.Parser{``0,``2})') if you need to capture the result

<a name='M-Monda-ParserExtensions-ManyUntil``3-Monda-Parser{``0,``1},Monda-Parser{``0,``2},System-Int32-'></a>
### ManyUntil\`\`3(self,next,min) `method`

##### Summary

Create a new parser that executes the current parser until another parser succeeds, then aggregates and combines the results

##### Returns

A new [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that executes the current parser until `next` succeeds, aggregating and combining the results

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| self | [Monda.Parser{\`\`0,\`\`1}](#T-Monda-Parser{``0,``1} 'Monda.Parser{``0,``1}') | The current parser |
| next | [Monda.Parser{\`\`0,\`\`2}](#T-Monda-Parser{``0,``2} 'Monda.Parser{``0,``2}') | The parser that determines when to stop repeating |
| min | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The minimum number of times the parser must execute |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |
| TResult | The result type of `self` |
| TNext | The result type of `next` |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.StackOverflowException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.StackOverflowException 'System.StackOverflowException') | If the current parser returns a zero-length result |

##### See Also

- [Monda.ParserExtensions.Repeat\`\`2](#M-Monda-ParserExtensions-Repeat``2-Monda-Parser{``0,``1},System-Int32- 'Monda.ParserExtensions.Repeat``2(Monda.Parser{``0,``1},System.Int32)')
- [Monda.ParserExtensions.Many\`\`2](#M-Monda-ParserExtensions-Many``2-Monda-Parser{``0,``1},System-Int32,System-Nullable{System-Int32}- 'Monda.ParserExtensions.Many``2(Monda.Parser{``0,``1},System.Int32,System.Nullable{System.Int32})')
- [Monda.ParserExtensions.Skip\`\`2](#M-Monda-ParserExtensions-Skip``2-Monda-Parser{``0,``1},System-Int32- 'Monda.ParserExtensions.Skip``2(Monda.Parser{``0,``1},System.Int32)')
- [Monda.ParserExtensions.SkipMany\`\`2](#M-Monda-ParserExtensions-SkipMany``2-Monda-Parser{``0,``1},System-Int32,System-Nullable{System-Int32}- 'Monda.ParserExtensions.SkipMany``2(Monda.Parser{``0,``1},System.Int32,System.Nullable{System.Int32})')
- [Monda.ParserExtensions.SkipUntil\`\`3](#M-Monda-ParserExtensions-SkipUntil``3-Monda-Parser{``0,``1},Monda-Parser{``0,``2},System-Int32- 'Monda.ParserExtensions.SkipUntil``3(Monda.Parser{``0,``1},Monda.Parser{``0,``2},System.Int32)')

<a name='M-Monda-ParserExtensions-Many``2-Monda-Parser{``0,``1},System-Int32,System-Nullable{System-Int32}-'></a>
### Many\`\`2(self,min,max) `method`

##### Summary

Create a new parser that executes the current parser until failure or an upper bound is reached and aggregates the results

##### Returns

A new [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that executes the current parser between `min` and `max` times and aggregates the results

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| self | [Monda.Parser{\`\`0,\`\`1}](#T-Monda-Parser{``0,``1} 'Monda.Parser{``0,``1}') | The current parser |
| min | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The minimum number of times the parser must execute (inclusive) |
| max | [System.Nullable{System.Int32}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.Int32}') | The maximum number of times the parser can execute (inclusive). A null value indicates there is no upper bound |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |
| TResult | The result type of `self` |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.StackOverflowException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.StackOverflowException 'System.StackOverflowException') | If the current parser returns a zero-length result |

##### See Also

- [Monda.ParserExtensions.Repeat\`\`2](#M-Monda-ParserExtensions-Repeat``2-Monda-Parser{``0,``1},System-Int32- 'Monda.ParserExtensions.Repeat``2(Monda.Parser{``0,``1},System.Int32)')
- [Monda.ParserExtensions.ManyUntil\`\`3](#M-Monda-ParserExtensions-ManyUntil``3-Monda-Parser{``0,``1},Monda-Parser{``0,``2},System-Int32- 'Monda.ParserExtensions.ManyUntil``3(Monda.Parser{``0,``1},Monda.Parser{``0,``2},System.Int32)')
- [Monda.ParserExtensions.Skip\`\`2](#M-Monda-ParserExtensions-Skip``2-Monda-Parser{``0,``1},System-Int32- 'Monda.ParserExtensions.Skip``2(Monda.Parser{``0,``1},System.Int32)')
- [Monda.ParserExtensions.SkipMany\`\`2](#M-Monda-ParserExtensions-SkipMany``2-Monda-Parser{``0,``1},System-Int32,System-Nullable{System-Int32}- 'Monda.ParserExtensions.SkipMany``2(Monda.Parser{``0,``1},System.Int32,System.Nullable{System.Int32})')
- [Monda.ParserExtensions.SkipUntil\`\`3](#M-Monda-ParserExtensions-SkipUntil``3-Monda-Parser{``0,``1},Monda-Parser{``0,``2},System-Int32- 'Monda.ParserExtensions.SkipUntil``3(Monda.Parser{``0,``1},Monda.Parser{``0,``2},System.Int32)')

<a name='M-Monda-ParserExtensions-Map``3-Monda-Parser{``0,``1},Monda-MapFunction{``0,``1,``2}-'></a>
### Map\`\`3(self,map) `method`

##### Summary

Create a new parser that maps the result of the current parser to a new value

##### Returns

A new [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that executes the current parser and maps the value to a new value

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| self | [Monda.Parser{\`\`0,\`\`1}](#T-Monda-Parser{``0,``1} 'Monda.Parser{``0,``1}') | The current parser |
| map | [Monda.MapFunction{\`\`0,\`\`1,\`\`2}](#T-Monda-MapFunction{``0,``1,``2} 'Monda.MapFunction{``0,``1,``2}') | A function that maps the value from the current parser to a new value |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |
| TResult | The result type of `self` |
| TNext | The mapped value type |

<a name='M-Monda-ParserExtensions-Optional``2-Monda-Parser{``0,``1},``1-'></a>
### Optional\`\`2(self,defaultValue) `method`

##### Summary

Create a parser that returns a zero-length default result if the current parser fails

##### Returns

A new [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that executes the current parser, returning its result when successful or a default zero-length result otherwise

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| self | [Monda.Parser{\`\`0,\`\`1}](#T-Monda-Parser{``0,``1} 'Monda.Parser{``0,``1}') | The current parser |
| defaultValue | [\`\`1](#T-``1 '``1') | The default value to return if the current parser fails |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |
| TResult | The result type of `self` |

<a name='M-Monda-ParserExtensions-Or``2-Monda-Parser{``0,``1},Monda-Parser{``0,``1}-'></a>
### Or\`\`2(self,other) `method`

##### Summary

Create a new parser that executes the current parser, or `other` if the current parser fails

##### Returns

A new [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that returns the first successful result of the current parser and `other` in that order

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| self | [Monda.Parser{\`\`0,\`\`1}](#T-Monda-Parser{``0,``1} 'Monda.Parser{``0,``1}') | The current parser |
| other | [Monda.Parser{\`\`0,\`\`1}](#T-Monda-Parser{``0,``1} 'Monda.Parser{``0,``1}') | The other parser to execute if the current one is fails |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |
| TResult | The result type of `self` and `other` |

<a name='M-Monda-ParserExtensions-PrecededBy``3-Monda-Parser{``0,``1},Monda-Parser{``0,``2}-'></a>
### PrecededBy\`\`3(self,previous) `method`

##### Summary

Create a new parser that ensures that the current parser is preceded by `previous`

##### Returns

A new [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that executes `previous` and `self` in that order and returns the `TResult` from `self`

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| self | [Monda.Parser{\`\`0,\`\`1}](#T-Monda-Parser{``0,``1} 'Monda.Parser{``0,``1}') | The current parser |
| previous | [Monda.Parser{\`\`0,\`\`2}](#T-Monda-Parser{``0,``2} 'Monda.Parser{``0,``2}') | The parser to execute before the current parser, its result will be discarded |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |
| TResult | The result type of `self` |
| TPrevious | The result type of `previous` |

##### Remarks

Although the parsing position will be advanced accordingly, the result of `previous` will be discarded. Use [Then\`\`3](#M-Monda-ParserExtensions-Then``3-Monda-Parser{``0,``1},Monda-Parser{``0,``2}- 'Monda.ParserExtensions.Then``3(Monda.Parser{``0,``1},Monda.Parser{``0,``2})') if you need to capture the result

<a name='M-Monda-ParserExtensions-Repeat``2-Monda-Parser{``0,``1},System-Int32-'></a>
### Repeat\`\`2(self,count) `method`

##### Summary

Create a new parser that executes the current parser a fixed number of times and aggregates the results

##### Returns

A new [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that executes the current parser `count` times and aggregates the results

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| self | [Monda.Parser{\`\`0,\`\`1}](#T-Monda-Parser{``0,``1} 'Monda.Parser{``0,``1}') | The current parser |
| count | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The number of times to repeat the current parser |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |
| TResult | The result type of `self` |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.StackOverflowException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.StackOverflowException 'System.StackOverflowException') | If the current parser returns a zero-length result |

##### See Also

- [Monda.ParserExtensions.Skip\`\`2](#M-Monda-ParserExtensions-Skip``2-Monda-Parser{``0,``1},System-Int32- 'Monda.ParserExtensions.Skip``2(Monda.Parser{``0,``1},System.Int32)')
- [Monda.ParserExtensions.Many\`\`2](#M-Monda-ParserExtensions-Many``2-Monda-Parser{``0,``1},System-Int32,System-Nullable{System-Int32}- 'Monda.ParserExtensions.Many``2(Monda.Parser{``0,``1},System.Int32,System.Nullable{System.Int32})')
- [Monda.ParserExtensions.ManyUntil\`\`3](#M-Monda-ParserExtensions-ManyUntil``3-Monda-Parser{``0,``1},Monda-Parser{``0,``2},System-Int32- 'Monda.ParserExtensions.ManyUntil``3(Monda.Parser{``0,``1},Monda.Parser{``0,``2},System.Int32)')
- [Monda.ParserExtensions.SkipMany\`\`2](#M-Monda-ParserExtensions-SkipMany``2-Monda-Parser{``0,``1},System-Int32,System-Nullable{System-Int32}- 'Monda.ParserExtensions.SkipMany``2(Monda.Parser{``0,``1},System.Int32,System.Nullable{System.Int32})')
- [Monda.ParserExtensions.SkipUntil\`\`3](#M-Monda-ParserExtensions-SkipUntil``3-Monda-Parser{``0,``1},Monda-Parser{``0,``2},System-Int32- 'Monda.ParserExtensions.SkipUntil``3(Monda.Parser{``0,``1},Monda.Parser{``0,``2},System.Int32)')

<a name='M-Monda-ParserExtensions-SkipMany``2-Monda-Parser{``0,``1},System-Int32,System-Nullable{System-Int32}-'></a>
### SkipMany\`\`2(self,min,max) `method`

##### Summary

Create a new parser that executes the current parser until failure or an upper bound is reached and counts the number of results

##### Returns

A new [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that executes the current parser between `min` and `max` times and counts the number results

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| self | [Monda.Parser{\`\`0,\`\`1}](#T-Monda-Parser{``0,``1} 'Monda.Parser{``0,``1}') | The current parser |
| min | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The minimum number of times the parser must execute (inclusive) |
| max | [System.Nullable{System.Int32}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.Int32}') | The maximum number of times the parser can execute (inclusive). A null value indicates there is no upper bound |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |
| TResult | The result type of `self` |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.StackOverflowException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.StackOverflowException 'System.StackOverflowException') | If the current parser returns a zero-length result |

##### See Also

- [Monda.ParserExtensions.Repeat\`\`2](#M-Monda-ParserExtensions-Repeat``2-Monda-Parser{``0,``1},System-Int32- 'Monda.ParserExtensions.Repeat``2(Monda.Parser{``0,``1},System.Int32)')
- [Monda.ParserExtensions.Many\`\`2](#M-Monda-ParserExtensions-Many``2-Monda-Parser{``0,``1},System-Int32,System-Nullable{System-Int32}- 'Monda.ParserExtensions.Many``2(Monda.Parser{``0,``1},System.Int32,System.Nullable{System.Int32})')
- [Monda.ParserExtensions.ManyUntil\`\`3](#M-Monda-ParserExtensions-ManyUntil``3-Monda-Parser{``0,``1},Monda-Parser{``0,``2},System-Int32- 'Monda.ParserExtensions.ManyUntil``3(Monda.Parser{``0,``1},Monda.Parser{``0,``2},System.Int32)')
- [Monda.ParserExtensions.Skip\`\`2](#M-Monda-ParserExtensions-Skip``2-Monda-Parser{``0,``1},System-Int32- 'Monda.ParserExtensions.Skip``2(Monda.Parser{``0,``1},System.Int32)')
- [Monda.ParserExtensions.SkipUntil\`\`3](#M-Monda-ParserExtensions-SkipUntil``3-Monda-Parser{``0,``1},Monda-Parser{``0,``2},System-Int32- 'Monda.ParserExtensions.SkipUntil``3(Monda.Parser{``0,``1},Monda.Parser{``0,``2},System.Int32)')

<a name='M-Monda-ParserExtensions-SkipUntil``3-Monda-Parser{``0,``1},Monda-Parser{``0,``2},System-Int32-'></a>
### SkipUntil\`\`3(self,next,min) `method`

##### Summary

Create a new parser that executes the current parser until another parser succeeds, then counts and combines the results

##### Returns

A new [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that executes the current parser until `next` succeeds, counting and combining the results

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| self | [Monda.Parser{\`\`0,\`\`1}](#T-Monda-Parser{``0,``1} 'Monda.Parser{``0,``1}') | The current parser |
| next | [Monda.Parser{\`\`0,\`\`2}](#T-Monda-Parser{``0,``2} 'Monda.Parser{``0,``2}') | The parser that determines when to stop repeating |
| min | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The minimum number of times the parser must execute |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |
| TResult | The result type of `self` |
| TNext | The result type of `next` |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.StackOverflowException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.StackOverflowException 'System.StackOverflowException') | If the current parser returns a zero-length successful result |

##### See Also

- [Monda.ParserExtensions.Repeat\`\`2](#M-Monda-ParserExtensions-Repeat``2-Monda-Parser{``0,``1},System-Int32- 'Monda.ParserExtensions.Repeat``2(Monda.Parser{``0,``1},System.Int32)')
- [Monda.ParserExtensions.Many\`\`2](#M-Monda-ParserExtensions-Many``2-Monda-Parser{``0,``1},System-Int32,System-Nullable{System-Int32}- 'Monda.ParserExtensions.Many``2(Monda.Parser{``0,``1},System.Int32,System.Nullable{System.Int32})')
- [Monda.ParserExtensions.ManyUntil\`\`3](#M-Monda-ParserExtensions-ManyUntil``3-Monda-Parser{``0,``1},Monda-Parser{``0,``2},System-Int32- 'Monda.ParserExtensions.ManyUntil``3(Monda.Parser{``0,``1},Monda.Parser{``0,``2},System.Int32)')
- [Monda.ParserExtensions.Skip\`\`2](#M-Monda-ParserExtensions-Skip``2-Monda-Parser{``0,``1},System-Int32- 'Monda.ParserExtensions.Skip``2(Monda.Parser{``0,``1},System.Int32)')
- [Monda.ParserExtensions.SkipMany\`\`2](#M-Monda-ParserExtensions-SkipMany``2-Monda-Parser{``0,``1},System-Int32,System-Nullable{System-Int32}- 'Monda.ParserExtensions.SkipMany``2(Monda.Parser{``0,``1},System.Int32,System.Nullable{System.Int32})')

<a name='M-Monda-ParserExtensions-Skip``2-Monda-Parser{``0,``1},System-Int32-'></a>
### Skip\`\`2(self,count) `method`

##### Summary

Create a new parser that executes the current parser a fixed number of times and counts the number of results

##### Returns

A new [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that executes the current parser `count` times, counting the number of results

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| self | [Monda.Parser{\`\`0,\`\`1}](#T-Monda-Parser{``0,``1} 'Monda.Parser{``0,``1}') | The current parser |
| count | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The number of times to repeat the current parser |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |
| TResult | The result type of `self` |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.StackOverflowException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.StackOverflowException 'System.StackOverflowException') | If the current parser returns a zero-length result |

##### See Also

- [Monda.ParserExtensions.Repeat\`\`2](#M-Monda-ParserExtensions-Repeat``2-Monda-Parser{``0,``1},System-Int32- 'Monda.ParserExtensions.Repeat``2(Monda.Parser{``0,``1},System.Int32)')
- [Monda.ParserExtensions.Many\`\`2](#M-Monda-ParserExtensions-Many``2-Monda-Parser{``0,``1},System-Int32,System-Nullable{System-Int32}- 'Monda.ParserExtensions.Many``2(Monda.Parser{``0,``1},System.Int32,System.Nullable{System.Int32})')
- [Monda.ParserExtensions.ManyUntil\`\`3](#M-Monda-ParserExtensions-ManyUntil``3-Monda-Parser{``0,``1},Monda-Parser{``0,``2},System-Int32- 'Monda.ParserExtensions.ManyUntil``3(Monda.Parser{``0,``1},Monda.Parser{``0,``2},System.Int32)')
- [Monda.ParserExtensions.SkipMany\`\`2](#M-Monda-ParserExtensions-SkipMany``2-Monda-Parser{``0,``1},System-Int32,System-Nullable{System-Int32}- 'Monda.ParserExtensions.SkipMany``2(Monda.Parser{``0,``1},System.Int32,System.Nullable{System.Int32})')
- [Monda.ParserExtensions.SkipUntil\`\`3](#M-Monda-ParserExtensions-SkipUntil``3-Monda-Parser{``0,``1},Monda-Parser{``0,``2},System-Int32- 'Monda.ParserExtensions.SkipUntil``3(Monda.Parser{``0,``1},Monda.Parser{``0,``2},System.Int32)')

<a name='M-Monda-ParserExtensions-Then``3-Monda-Parser{``0,``1},Monda-Parser{``0,``2}-'></a>
### Then\`\`3(self,next) `method`

##### Summary

Create a new parser that executes the current parser and `next` in order and combines the results

##### Returns

A new [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that executes `self` and `next` is sequence and returns a [Tuple\`2](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Tuple`2 'System.Tuple`2') of the results if both are successful

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| self | [Monda.Parser{\`\`0,\`\`1}](#T-Monda-Parser{``0,``1} 'Monda.Parser{``0,``1}') | The current parser |
| next | [Monda.Parser{\`\`0,\`\`2}](#T-Monda-Parser{``0,``2} 'Monda.Parser{``0,``2}') | The next [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |
| TResult | The result type of the current parser |
| TNext | The result type of `next` |

<a name='M-Monda-ParserExtensions-TryMap``3-Monda-Parser{``0,``1},Monda-TryMapFunction{``0,``1,``2}-'></a>
### TryMap\`\`3(self,tryMap) `method`

##### Summary

Create a new parser that tries to map the result of the current parser to a new value

##### Returns

A new [Parser\`2](#T-Monda-Parser`2 'Monda.Parser`2') that executes the current parser and tries to map the value to a new value

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| self | [Monda.Parser{\`\`0,\`\`1}](#T-Monda-Parser{``0,``1} 'Monda.Parser{``0,``1}') | The current parser |
| tryMap | [Monda.TryMapFunction{\`\`0,\`\`1,\`\`2}](#T-Monda-TryMapFunction{``0,``1,``2} 'Monda.TryMapFunction{``0,``1,``2}') | A function that tries to map the value from the current parser to a new value |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |
| TResult | The result type of `self` |
| TNext | The mapped value type |

<a name='T-Monda-ParserPredicate`1'></a>
## ParserPredicate\`1 `type`

##### Namespace

Monda

##### Summary

Predicate that tests the `TSource` item at the current index.

##### Returns

True if the `TSource` item at the current index is acceptable, false otherwise

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| data | [T:Monda.ParserPredicate\`1](#T-T-Monda-ParserPredicate`1 'T:Monda.ParserPredicate`1') | Input data to parse |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |

<a name='T-Monda-Parser`2'></a>
## Parser\`2 `type`

##### Namespace

Monda

##### Summary

Fundamental parsing unit to extract one `TResult` from a set of `TSource` input items

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TSource | The type of items in the input data |
| TResult | The result type of the parser |

<a name='P-Monda-Parser`2-Name'></a>
### Name `property`

##### Summary

Identifying name for the parser

<a name='T-Monda-Range'></a>
## Range `type`

##### Namespace

Monda

##### Summary

Represents start and length properties that define a range in contiguous data
