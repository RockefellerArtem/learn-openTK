namespace openTKLearn
{
    public class Program
    {
        private static void Main(string[] args)
        {
            using var game = new Game(500, 500);
            
            game.Run();
        }
    }
}
