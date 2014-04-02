namespace AST 

type Val( intVal:    int64 option,
          floatVal:  float option,
          stringVal: string option,
          charVal:   char option,
          nullVal:   unit option,
          boolVal:   bool option ) =
   class
        member x.Int    = intVal
        member x.Float  = floatVal
        member x.String = stringVal
        member x.Char   = charVal
        member x.Null   = nullVal
        member x.Bool   = boolVal
        
        new(value : int64)  = Val(Some value, None, None, None, None, None)
        new(value : float)  = Val(None, Some value, None, None, None, None)
        new(value : string) = Val(None, None, Some value, None, None, None)
        new(value : char)   = Val(None, None, None, Some value, None, None)
        new(value : unit)   = Val(None, None, None, None, Some value, None) // null = Val(unit)
        new(value : bool)   = Val(None, None, None, None, None, Some value)

        new()   = Val(None, None, None, None, None, None)
   end