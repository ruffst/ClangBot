using System;
using System.Net.Http;
using System.Text.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ClangBot
{
    public partial class Form1 : Form
    {
        private string apiEndpoint = "https://api.openai.com/v1/chat/completions";
        private HttpClient client;
        private string apiKey = "";
        private string knowledgeBase = ""; // Hier können Sie den Inhalt Ihrer Textdatei laden, z.B. mit File.ReadAllText("path_to_your_file.txt");
        private string characterPrompt = "Du bist ein sarkastischer Gott, der technisches Wissen über Space Engineers hat. Deine Antwort sollte auch nicht länger als 200 Zeichen sein.";


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBoxLog.Text = "";
            richTextBoxLog.AppendText("Bot gestartet" + Environment.NewLine);
            button1.Enabled = false;
            button2.Enabled = true;

            if (apiKey != "")
            {
                connectToOpenAI(apiKey);
                listenToFileAsync();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;

            if (File.Exists("api.key"))
            {
                apiKey = File.ReadAllText("api.key");
                textBoxApiKey.Text = apiKey;
            }

            client = new HttpClient();



        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string contentToSave = textBoxApiKey.Text;
            File.WriteAllText("api.key", contentToSave);
        }

        private void connectToOpenAI(string apiKey)
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            richTextBoxLog.AppendText("connected To OpenAI" + Environment.NewLine);
        }

        private async Task listenToFileAsync()
        {
            while (true)
            {
                if (!button1.Enabled)
                {
                    richTextBoxLog.AppendText("waiting for File" + Environment.NewLine);
                    if (File.Exists("chat.txt"))
                    {
                        //richTextBoxLog.AppendText("chat.txt gefunden" + Environment.NewLine);
                        // Read chat.txt without locking the file
                        using FileStream fs = new FileStream("chat.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        using StreamReader sr = new StreamReader(fs, Encoding.UTF8);
                        string text = await sr.ReadToEndAsync();



                        if (text.Contains("Klang", StringComparison.OrdinalIgnoreCase) || text.Contains("Clang", StringComparison.OrdinalIgnoreCase))
                        {

                            richTextBoxLog.AppendText(text + Environment.NewLine);

                            if (!string.IsNullOrWhiteSpace(text))
                            {
                                // Clear the chat.txt file content
                                await File.WriteAllTextAsync("chat.txt", string.Empty);

                                var requestData = new
                                {
                                    model = "gpt-3.5-turbo",
                                    messages = new[]
                                    {
                                        new { role = "system", content = knowledgeBase }, // Wissensbasis
                                        new { role = "user", content = characterPrompt + text } // Charakteranweisung + eigentliche Frage

                                    }
                                };


                                var response = await client.PostAsync(apiEndpoint, new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json"));
                                var responseBody = await response.Content.ReadAsStringAsync();
                                var result = JsonSerializer.Deserialize<ResponseData>(responseBody);

                                //Console.WriteLine($"Response: {result.ToString()}");
                                //richTextBoxLog.AppendText(responseBody + Environment.NewLine);
                                richTextBoxLog.AppendText(result.choices[0].message.content.Trim() + Environment.NewLine);

                                // Write response to Response.txt
                                await File.WriteAllTextAsync("Response.txt", result.choices[0].message.content.Trim());


                            }
                        }
                    }
                    await Task.Delay(1000);  // Wait for 1 second
                }else
                {
                    return;
                }
            }
        }

    }

    public class ResponseData
    {
        public Choice[] choices { get; set; }

        public class Choice
        {
            public Message message { get; set; }

            public class Message
            {
                public string role { get; set; }
                public string content { get; set; }
            }
        }
    }
}