using static System.Net.Mime.MediaTypeNames;

namespace RandomText
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string text = "";

            // Write messages 
            for (int i = 0; i < 32; i++)
            {
                text += $"\n[msg CorruptedText{i}[{RandomString(15)}]]";
                Random rnd = new Random();
                for (int x = 0; x < rnd.Next(1, 3); x++)
                    text += RandomMessagebox(true, 45);
                text += "\n";
            }

            // Write selections
            for (int i = 0; i < 50; i++)
            {
                text += $"\n[sel CorruptedSel{i}]";
                Random rnd = new Random();
                for (int x = 0; x < rnd.Next(1, 3); x++)
                    text += RandomMessagebox(false, 20);
                text += "\n";
            }

            File.WriteAllText("output.txt", text);
        }

        private static string RandomMessagebox(bool waitEnd = true, int maxLength = 30)
        {
            string text = "";
            Random rnd = new Random();
            for (int x = 0; x < rnd.Next(1, 3); x++)
            {
                int length = rnd.Next(0, maxLength);
                text += $"\n{RandomString(length, waitEnd)}";
                if (waitEnd)
                    text += "[n]";
                else
                    text += "[e]";
            }
            if (waitEnd)
                text += "\n[w][e]";
            return text;
        }

        static string RandomString(int maxLength = 30, bool color = false)
        {
            Random rnd = new Random();
            string text = "";

            int numberOfCharacters = rnd.Next(1, maxLength + 1);
            string characters = "よらりるれろゎわゐゑをんァアィイゥウェエォオカガキギクグケゲコゴサザシジスズセゼソゾタダチヂッツヅテデトドナニヌネノハバパヒビピフブプヘベペホボポマミムメモャヤュユョヨラリルレロヮワヰヱヲンヴヵヶ";

            for (int i = 0; i < numberOfCharacters; i++)
            {
                int index = rnd.Next(characters.Length);
                text += characters[index];

                if (color && rnd.Next(0, 4) == 1)
                    text += $"[clr {rnd.Next(0, 29)}]";
            }

            return text;
        }
    }
}
