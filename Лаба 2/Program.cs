using System;
using System.Threading;//new

namespace Лаба_2
{
    class Program
    {
        static void Main(string[] args)
        {
            char[] alfabet = new char[] { '0', '1', '_' };
            var machine = new Machine
            (
                new TransitionMatrix
                (
                    new Row[]
                    {
                        new Row('0', new Transition[]{new Transition('1', Move.Right, 0) }),
                        new Row('1', new Transition[]{new Transition('0', Move.Right, 0) }),
                        new Row('_', new Transition[]{new Transition('_', Move.Right, -1)})
                    }
                )
            );
            machine.SetHead(machine.sHead);
            machine.State = 0;
            string str = "11001100";
            str = machine.Audit(str, alfabet);
            machine.Input(0, str);
            int time = 1000;//new

            int temp = 0;
            
            Console.WriteLine(machine.ShowHead(0, str.Length + 1, temp));
            Console.WriteLine(machine.ShowString(0, str.Length+1));
            
            while(machine.State != -1)
            {
                Thread.Sleep(time);//new
                Console.Clear();//new
                machine.Run();
                Console.WriteLine(machine.ShowHead(0, str.Length + 1, temp));
                Console.WriteLine(machine.ShowString(0, str.Length + 1));
                Console.WriteLine("");
                Thread.Sleep(time);//new
                Console.Clear();//new


                Console.WriteLine(machine.ShowHead(0, str.Length + 1, temp+1));
                Console.WriteLine(machine.ShowString(0, str.Length + 1));
                temp++;
            }
        }
    }
    class Transition
    {
        public char replacement;
        public Move move;
        public int newState;
        public Transition(char replacement, Move move, int newState)
        {
            this.replacement = replacement;
            this.move = move;
            this.newState = newState;
        }

    }
    internal enum Move
    {
        Left,
        Right
    }
    class Row
    {
        public char symbol;
        public Transition[] transitions;
        public Row(char symbol, Transition[] transitions)
        {
            this.symbol = symbol;
            this.transitions = transitions;
        }
    }
    class TransitionMatrix
    {
        public Row[] rows;
        public TransitionMatrix(Row[] rows)
        {
            this.rows = rows;
        }
        public Transition GetRow(char symbol, int state)
        {
            foreach (var row in rows)
            {
                if (row.symbol == symbol)
                {
                    return row.transitions[state];
                }

            }
            throw new NotImplementedException();
        }
    }
    class Machine
    {
        public int sHead = 0;
        public int Head { get; set; }
        public int State { get; set; }
        public char[] tape = new char[5];
        public char[] headPos = new char[5];

        private readonly TransitionMatrix _transitionMatrix;
        public Machine(TransitionMatrix transitionMatrix)
        {
            _transitionMatrix = transitionMatrix;
            Array.Fill(tape, '_');
            Array.Fill(headPos, '_');
        }
        public void Input(int index, string str)
        {
            Point1:
            for (int i = 0; i < str.Length; i++)
            {
                if (i + index + tape.Length / 4 < str.Length)
                {
                    Size();
                    goto Point1;
                }
                tape[i + index + tape.Length / 4] = str[i];
            }

            headPos = new char[tape.Length];
            Array.Fill(headPos, '_');
            headPos[tape.Length / 4] = '↓';
        }
        public string ShowString(int index1, int length)
        {
            return new string(tape, index1 + tape.Length / 4, length);
        }
        public string ShowHead(int index, int length, int temp)
        {
            if (temp == 0)
            {
                return new string(headPos, index + tape.Length / 4, length);
            }else if (temp >= 1)
            {

                headPos[temp - 1 + index + tape.Length / 4] = '_';
                headPos[temp + index + tape.Length / 4] = '↓';

                return new string(headPos, index + tape.Length / 4, length);
            }
           
            return new string(headPos, index + tape.Length / 4, length);
        }

        public void Run()
        {
    
                Transition transition = _transitionMatrix.GetRow(tape[Head], State);
                tape[Head] = transition.replacement;
                if (transition.move == Move.Left)
                {
                    Head--;
                    if (Head < 0)
                    {
                        Size();
                        
                    }
                }
                else if (transition.move == Move.Right)
                {
                    Head++;
                    if (Head >= tape.Length)
                    {
                        Size();
                    }
            }
                else
                {
                    throw new NotImplementedException();
                }
                State = transition.newState;
            
        }
        public void SetHead(int head)
        {
            Head = head + tape.Length / 4;

        }
        public void Size()
        {
            char[] tmp = tape;
            tape = new char[tmp.Length * 2];
            Array.Fill(tape, '_');
            for (int i = tape.Length / 4; i < tmp.Length + tape.Length / 4; i++)
            {
                tape[i] = tmp[i - tape.Length / 4];

            }
            SetHead(sHead);

            headPos = new char[tape.Length];
            Array.Fill(tape, '_');
            headPos[tape.Length / 4] = '↓';
        }
        public string Audit(string sTape, char[] alfabet)
        {
            for (int i = 0; i < sTape.Length; i++)
            {
                for (int j = 0; j < alfabet.Length; j++)
                {
                    if (sTape[i] == alfabet[j])
                    {
                        break;
                    }
                    else if (j == alfabet.Length - 1 && sTape[i] != alfabet[j])
                    {
                        Console.WriteLine("Введені данні не коректні");
                        Console.WriteLine("Буде використано стандартна стрічка  \"1001100\"");
                        sTape = "1001100";
                        return sTape;
                    }
                }

            }
            return sTape;
        }
    }
}


