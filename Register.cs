using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;
using System.Threading;
using System.Linq;

namespace Hamnen
{
    class Register
    {
        const int xPosHmReg = 50, yPosHmReg = 7;

       public static List<Båt> HamnRegister = new List<Båt>();   // registret av hamnen i vilken alla inkommande båtar läggs in. ( i början är tom)
        public static void SkrivutHamnaregister(int day)  

        {
 /* var = IEnumerable*/   var dagensRegister = from b in HamnRegister   // dagenRegister är en lista , det finns alla båtar som är i hamnen en x dag.   
                    where (day - b.aDag) < b.dagarIhamnen     // (pågående dag - ankomstdagen av båten) ska vara < dagarna båten ska stanna i hamnen
                    orderby b.kajPlats
                    select b;
            
           // flotta(day);      // räcknar båtarna enligt typ som finns i kajen en x dag   (ex. Motorbåtar[5])
           // statistik(day);
            
            int ypos = yPosHmReg;
            SetCursorPosition(xPosHmReg, ypos++);
            Console.BackgroundColor = ConsoleColor.Blue;
            Write("       H A M N R E G I S T R E T       ");
            Console.BackgroundColor = ConsoleColor.Black;
            
            string sPlats, övrigt;
            foreach (var b in dagensRegister)       // skriver ut alla båtar som finns i listan q 
            {
                sPlats = b.kajPlats.ToString();
                if (b.antalPlatser > 1)
                    sPlats += "-" + (b.kajPlats + b.antalPlatser - 1).ToString();
                //sPlats += "".Substring(0, b.antalPlatser - 1);

                övrigt = b.övrigt.Beskrivning + b.övrigt.value.ToString() + b.övrigt.mått;

                SetCursorPosition(xPosHmReg, ypos++);

                Write("{0,-5} [{1,5}] ", sPlats, b.Identitetsnummer);
                Console.ForegroundColor = b.färg;
                Write(" {0,-10}  ", b.typ);
                Console.ForegroundColor = ConsoleColor.White;
                Write("{0,-15} ", övrigt);
           
            }
            for (int i = 1; i < 7 && ypos < 40; i++)
            {
                SetCursorPosition(xPosHmReg, ypos++);
                WriteLine($"                                                                   ");
            }
        }
        public static void flotta(int day)
        {
            var q1 = from b in HamnRegister         // delat i grupp alla båttyper.   q1 är 4 tabeller dvs en tabel för varje båt i vilken finns alla egenskaper av varje båt.
                     where (day - b.aDag ) < b.dagarIhamnen
                     group b by b.typ;

            SetCursorPosition(85, 4);

            int totalBåtar = (from t in q1
                             select t.Count()).Sum();         // från t (tabell) som finns i q1, (count) räcknar alla båtar som är i varje tabell.  Sum()  summerar

            foreach (var z in q1)
            {
                Write("{0,2} [{1,2}]", z.Key, z.Count());
            }

            Console.BackgroundColor = ConsoleColor.Blue;
            SetCursorPosition(85, 3);
            Write($" Båtar som finns på kajen {totalBåtar}");
            Console.BackgroundColor = ConsoleColor.Black;
        }
        public static void statistik(int day)    
        {
            var totalVikt = (from b in HamnRegister
                     where (day - b.aDag) < b.dagarIhamnen
                     select b.vikt).Sum();


            var medelHastighet = (from b in HamnRegister
                                  where (day - b.aDag) < b.dagarIhamnen
                                  select b.maxHastighet).Average();

            double ledigaPlatser = Kaj.ledigaPlatser();

            Console.SetCursorPosition(2, 36);
           // Console.Write($"Lediga Platser: {ledigaPlatser}  Total Vikt: {totalVikt}  Medelhastighet: {medelHastighet}  ");
            Console.Write("Lediga Platser: {0,2}  Total Vikt: {1,-7}  Medelhastighet: {2, 6:N1}  ", ledigaPlatser, totalVikt, medelHastighet);

        }
    }
}
