
Console.Write("Mettre une Valeur : ");
string value = Console.ReadLine();
if (!int.TryParse(value, out int result) || string.IsNullOrWhiteSpace(value) || result < 0)
{
    Console.WriteLine("Il faut un nombre positif. Il faut mettre une valeur");
}
else
{
    for (int i = 0; i <= 12; i++)
    {
        Console.Write(Fibonnaci(i) + " ");
    }
}
static int Fibonnaci(int number)
{
    if (number < 2)
        return number;
    return Fibonnaci(number - 1) + Fibonnaci(number - 2);
}





//TestFibonnaci(0, 1, 144);

//static void TestFibonnaci(int a, int b, int number)
//{
//    Console.Write(a + " ");
//    if (b > number)
//    {
//        return;
//    }
//    TestFibonnaci(b, a + b, number);
//}

