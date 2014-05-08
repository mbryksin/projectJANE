module internal SA.Dictionary

open System.Collections.Generic
open System

type KeyNotFound = KeyNotFoundException

// Дополнение для класса Dictionary
type Dictionary<'TKey, 'TValue> with
    
    /// <summary> 
    /// Добавляет указанные ключ и значение в словарь. 
    /// В случае повтора выполняет действие.
    /// </summary>
    member x.TryAddWithAction key value action = try  x.Add(key, value)
                                                 with :? ArgumentException -> action()
    /// <summary>
    /// Добавляет указанные ключ и значение в словарь.
    /// В случае повтора - бездействие.
    /// </summary>
    member x.TryAddWithInaction key value = x.TryAddWithAction key value id

    /// <summary> 
    /// Добавляет список указанных ключей и значений в словарь. 
    /// В случае повтора выполняет действие.
    /// </summary>
    member x.TryAddListWithAction list action = 
        List.iter (fun (a, b) -> x.TryAddWithAction a b action) list

    /// <summary>
    /// Добавляет список указанных ключей и значений в словарь. 
    /// В случае повтора - бездействие.
    /// </summary>
    member x.TryAddListWithInaction list = x.TryAddListWithAction list id

    /// <summary> 
    /// Возвращает Some значение по ключу.
    /// Возвращает None в случае отсутствия значения.
    /// </summary>
    member x.GetOption key = try Some x.[key]
                             with :? KeyNotFound -> None

    /// <summary> 
    /// Возвращает список значений.
    /// </summary>
    member x.ToListValues() = [for a in x -> a.Value]

    /// <summary> 
    /// Возвращает список ключей.
    /// </summary>
    member x.ToListKeys() = [for a in x -> a.Key]
    