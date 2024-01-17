namespace MaM;

public static class Program
{
  private static void Main()
  {
    while (Menus.MainMenu.Show());

//TODO - don't forget to revise all uses of 'ref' - only fo it if the value is gonna change or if the object is too large and a copy is not ideal.
  }
}