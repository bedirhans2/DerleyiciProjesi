using System;

namespace DerleyiciProje
{

    /* Gramer: 
P →  {C} '.'
C  → I | W | A | Ç | G 
I   → '[' E '?'  C{ C } ':' C{ C } ']' | '[' E '?'  C{C} ']'
W → '{' E '?'  C{C} '}'
A  → K '=' E ';'
Ç  → '<' E ';'
G → '>' K ';'
E → T {('+' | '-') T}
T → U {('*' | '/' | '%') U}
U → F '^' U | F
F → '(' E ')' | K | R
K → 'a'  |  'b'  | … |  'z' 
R → '0'  |  '1'  | … |  '9' 
 
P: program
C: Cümle
I: IF cümlesi
W: While döngüsü
A: Atama cümlesi
Ç: Çıktı cümlesi
G: Girdi cümlesi
E: Aritmetik İfade 
T: Çarpma-bölme-mod terimi
U: Üslü ifade
F: Gruplama ifadesi
K: Küçük harfler
R: Rakamlar

         
         */
    class Program
    {
        public static int sayac = 0; //sayacı tokenleri ilerletme için kullanıyoruz
        public static string token = "";//input için tokenimiz tanımlandı
        public static char[] TokenGetir; //getToken'i tanımladık. için dizi olarak tanımladık
        static void Main(string[] args)
        {
            token = "n=9; {n-2^1><n;n=n+1}."; //Token için rastgele bir deneme stringi oluşturduk

            //Token için gerekli input stringini klavyeden de alabiliriz.

           // Console.WriteLine("Bir Input Stringi Giriniz: \n");
           // token= Console.ReadLine();

            Console.WriteLine("Input Stringi: " + token);//ifadeyi yazdırdık
            TokenGetir = token.ToCharArray();//tokeni char dizisine çevirdik. Çünkü tokenimizi getToken de kullanacağız. Böylece getToken() fonksiyonu yazmaya gerek kalmıyor
            P();//Gramer her zaman Programdan başlar
        }
        //Grameri oluşturalım
        //Gramerimiz P yani Program başlar
        //Gramer Programdan yani P() den başlar   
        public static void P()
        {
            if (token.Contains(" . "))
            {
                Environment.Exit(0);//Tokende . varsa, program biter
            }
            else
            {
                //eğer . yoksa, C fonksiyonuna yani Cümleye geç. Her paragraf cümlelerden oluşur.
              //  Console.WriteLine("Program: " + TokenGetir[sayac]);
                C();
            }
        }

        //Her program Cümlelerden yani C() den oluşur
        public static void C()
        {
            if (TokenGetir[sayac] == '[')
            {
                sayac = sayac + 1;//eğer input stringte '[' varsa, sonraki ifadeyi getirmek için sayaç değeri 1 artar
                Console.WriteLine("Cümle: ", TokenGetir[sayac]); //cümleyi yazdırdık
                I(); //if ifadesi çalışacak
            }
            else if (TokenGetir[sayac] == '(')
            {
                sayac = sayac + 1; ;//eğer input stringte '(' varsa, sonraki ifadeyi getirmek için sayaç değeri 1 artar
                W(); //while döngüsü çalışacak
            }
            bool dogru = false; //doğruluk kontrolü
            //isteneni bulmak için a-z arasındaki küçük harfleri getirdik. Bu harfler bizim terminal ifadelerimizdir.
            for (char i = 'a'; i <= 'z'; i++)
            {
                if (TokenGetir[sayac] == i)
                {
                    dogru = true;
                }
                if (dogru)
                {
                    Console.WriteLine("Cümle: " + TokenGetir[sayac]); //cümleyi yazdırdık
                    A(); //atama cümlesi çalışacak
                }
                if (TokenGetir[sayac] == '<')
                {
                    sayac = sayac + 1; //sayac++ ifadesi yerine yazabiliriz
                    Ç(); //çıktı cümlesi çalışacak
                }
                if (TokenGetir[sayac] == '>')
                {
                    sayac = sayac + 1;
                    G(); //girdi cümlesi çalışacak
                }
            }
        }



        //IF cümlesi tanımlandı
        public static void I()
        {
            E();//önce aritmetik ifade çalışır
            bool boo = false; //kontrol ifadesi
            //ifadede ? varsa, if ifadesi vardır
            if (TokenGetir[sayac] == '?')
            {
                Console.WriteLine("IF Cümlesi: " + TokenGetir[sayac]);
                boo = true;
            }
            if (!boo)
            {
                //kontrol ifademiz boo yoksa hata vererek programı kapatır.
                Console.Error.WriteLine("ERROR!");
                Environment.Exit(0);
            }
            else
            {
                //boo ifadesi varsa, onu bulana kadar tokeni 1 arttırır
                sayac = sayac + 1;
            }
            C();//Gramere göre en sonda cümle çalışır.

        }
        //While döngüsü fonksiyonu
        public static void W()
        {
            E(); //önce aritmetik ifade çalışacak
            bool doruluk = false;//doğruluk kontrolü için tanımladık
            //? yoksa, kontrol doğrudur ve while döngüsü başlar
            if (TokenGetir[sayac] != '?')
            {
                Console.WriteLine("While Döngüsü: " + TokenGetir[sayac]);
                doruluk = true;
            }
            if (doruluk)
            {
                sayac = sayac + 1;
            }
            //'}' varsa, program hata vererek biter
            if(TokenGetir[sayac] !='}'|| TokenGetir[sayac] != '{')
            {
                Console.WriteLine("Error!");
                Environment.Exit(0);
            }
            C(); //en son cümle çalışır
        }
        //Atama cümlesi tanımlandı
        public static void A()
        {
            bool d = false, d1 = false;//atamada 2 kontrol ifadesi var biri = yi biri ; yi kontrol eder
            K(); //önce küçük harfler ifadesi çalışacak

            //= varsa atama var
            if (TokenGetir[sayac] == '=')
            {
                Console.WriteLine("Atama Cümlesi: " + TokenGetir[sayac]);
                A();
                d = true;
            }
            if (d)
            {
                sayac = sayac + 1;
            }

            //; varsa atama var
            if (TokenGetir[sayac] == ';')
            {
                Console.WriteLine("Atama Cümlesi: " + TokenGetir[sayac]);

                d1 = true;
            }
            if (d1)
            {
                sayac = sayac + 1;
            }

        }
        //Çıktı cümlesi tanımlandı
        public static void Ç()
        {
            //”<” koşulu en başta belirtildiği için yazmadık
            E();//önce E çalışır. Bu gramerin kuralıdır.
            bool a = false; //Kontrol ifademiz
           
            //; varsa, sonraki tokene geçeriz
            if (TokenGetir[sayac] == ';')
            {
                a = true;
            }
            if (a)
            {
                sayac = sayac + 1;
            }

        }
        //girdi cümlesi tanımlandı
        public static void G()
        {    //”>” koşulu en başta belirtildiği için yazmadık
            K();//önce K çalışır. Bu gramerin kuralıdır.
            bool c = false; //kontrol ifadesi

            //; varsa, girdi sonraki tokenden alınır
            if (TokenGetir[sayac] == ';')
            {
                Console.WriteLine("Girdi Cümlesi: " + TokenGetir[sayac]);
            }
            if (c)
            {
                sayac = sayac + 1;
            }

        }
        //aritmetik ifade tanımlandı
        public static void E()
        {
            T(); //önce T çalışır. Bu gramerin kuralıdır.
            bool z = false; // kontrol ifadesi

            //+ ve - işlemi varsa aritmetik ifade çalışır
            if (TokenGetir[sayac] == '+' || TokenGetir[sayac] == '-')
            {
                z = true;
                Console.WriteLine("Aritmetik İfade: " + TokenGetir[sayac]);
            }
            if (z)
            {
                sayac = sayac + 1;
            }
            T();//en son T çalışır. Bu gramerin kuralıdır.

        }
        //Çarpma-Bölme-Modulo tanımlandı
        public static void T()
        {
            U(); //önce U çalışır. Bu gramerin kuralıdır.
            bool k = false;//control

            //* / ve % varsa, bunlar T olarak tanımlanır
            if (TokenGetir[sayac] == '*' || TokenGetir[sayac] == '/' || TokenGetir[sayac] == '%')
            {
                k = true;
                Console.WriteLine("Çarpma-Bölme-Mod ifadesi: " + TokenGetir[sayac]);
            }
            if (k)
            {
                sayac = sayac + 1;
            }
            U();
        }

        //Üslü ifadeleri tanımladık
        public static void U()
        {
            F(); //önce F çalışır. Bu gramerin kuralıdır.
            bool s = false;
            if (TokenGetir[sayac] == '^')//üs ifadesi varsa üs ifadesi çalışır
            {
                Console.WriteLine("Üs ifadesi: " + TokenGetir[sayac]);
                s = true;
            }
            if (s)
            {
                sayac = sayac + 1;
                U();
            }
            else
            {
                F();//program gruplama ile biter
            }

        }

        //Gruplama ifadelerini tanımladık
        public static void F()
        {
            bool s1 = false, s2 = false;
            if (TokenGetir[sayac] == '(' && TokenGetir[sayac] == ')') //’(‘ ve’)’ ifadeleri varsa, E() Çalışır.
            {
                E();//Aritmetik ifadeyle başlar
            }

            for (char i = '0'; i <= '9'; i++)
            { //ifadede rakam varsa, rakam ifadesinde çalışır ve gruplanır

                if (TokenGetir[sayac] == i)
                {
                    Console.WriteLine("Rakam ifadeleri: " + TokenGetir[sayac]);

                    s1 = true;
                }
            }
            if (s1)
            {
                R();//rakamlar çalışır
            }
            //ifadede küçük harf varsa, küçük harf ifadesinde çalışır ve gruplanır

            for (char i = 'a'; i <= 'z'; i++)
            {
                if (TokenGetir[sayac] == i)
                {
                    s2 = true;
                }
            }

            if (s2)
            {
                K();//küçük harfler
            }
        }

        //Küçük harfleri tanımladık
        public static void K()
        {
            for (char i = 'a'; i <= 'z'; i++)
            {
                //ifadede küçük harf varsa, küçük harf ifadesinde çalışır

                if (TokenGetir[sayac] == i)
                {
                    Console.WriteLine("küçük harf ifadeleri: " + TokenGetir[sayac]);
                    break;
                }
            }
            sayac = sayac + 1;

        }

        //Rakam ifadelerini tanımladık
        public static void R()
        {
            for (char i = '0'; i <= '9'; i++)
            {
                //ifadede rakam varsa, rakam ifadesinde çalışır
                if (TokenGetir[sayac] == i)
                {
                    Console.WriteLine("Rakam ifadeleri: " + TokenGetir[sayac]);
                }
            }
            sayac++;
        }

    }
}