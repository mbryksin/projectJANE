namespace SA

open SA.Program

// GD ~ Gathering Data
// SA ~ Static Analysis

type StaticAnalysis =
    static member Analyze program =
        // Сбор данных
        GD_Program program
        // Статический анализ
        SA_Program program 