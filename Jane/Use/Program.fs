open LanguageParser
open AST

let test = @"
    interface ISummator {
        void incr();
    }
    
    class Incrementer extends ISummator 
    {
        Incrementer(int startValue)
        {
            x = startValue;    
        }

        int x = 0;

        int getValue()
        {
            return x;
        }

        void incr()
        {
            x = x + 1;
        }
    }
    
    class MainClass
    {
	    static void main(String[] args)
	    {
            Incrementer i = new Incrementer(1);
            i.incr();
            i.getValue();
	    }
    }
"

let b = ParseProgram test
printfn "%A" b