using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using static Program;
public class Program
{

    static HttpClient client = new HttpClient();
    const string ENDPOINT_RESULTADOS_UCL = "https://jsonmock.hackerrank.com/api/football_matches";

    public class footballmatches
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total { get; set; }
        public int total_pages { get; set; }
        public List<data> data { get; set; }
    }

    public class data
    {
        public string competition { get; set; }
        public int year { get; set; }
        public string round { get; set; }
        public string team1 { get; set; }
        public string team2 { get; set; }
        public string team1goals { get; set; }
        public string team2goals { get; set; }
    }

    public static void Main()
    {
        Console.Write("Entre com o ANO que você deseja consultar os resultados: ");
        string year = Console.ReadLine();

        Console.Write("Entre com o nome do TIME 1 da partida: ");
        string team1 = Console.ReadLine();

        Console.Write("Entre com o nome do TIME 2 da partida: ");
        string team2 = Console.ReadLine();

        Console.Write("Entre o NÚMERO DA PÁGINA de resultados: ");
        string page = Console.ReadLine();

        getTotalScoredGoals(year, team1, team2, page).GetAwaiter().GetResult();
    }

    static async Task<footballmatches> getTotalScoredGoals(string year, string team1, string team2, string page)
    {
        var retornoAno = "";    // variável que controla se deve mostrar o ano ao final do resultado.
        var team1goals = 0;
        var team2goals = 0;


        var endpointConsulta = ENDPOINT_RESULTADOS_UCL + "?";

        if (!string.IsNullOrEmpty(year.Trim()))
        {
            retornoAno = " in " + year;
            endpointConsulta = endpointConsulta + $"year={year}&";
        }
           
        if (!string.IsNullOrEmpty(team1.Trim()))
            endpointConsulta = endpointConsulta + $"team1={team1}&";

        if (!string.IsNullOrEmpty(team2.Trim()))
            endpointConsulta = endpointConsulta + $"team2={team2}&";

        if (!string.IsNullOrEmpty(page.Trim()))
            endpointConsulta = endpointConsulta + $"page={page}&";
            

        HttpResponseMessage response = await client.GetAsync(endpointConsulta);


        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var responseJson = JsonSerializer.Deserialize<footballmatches>(responseBody);

            if (responseJson != null)
            {

                // team1
                if (!string.IsNullOrEmpty(team1.Trim()))
                {
                    var team1Results = responseJson.data.GroupBy(x => x.team1).ToList();
                    foreach (var result in team1Results)
                    {
                        foreach (var goal in result.Select(x => x.team1goals))
                        {
                            team1goals += int.Parse(goal);
                        }
                    }
                    Console.WriteLine("Team " + team1 + " scored " + team1goals.ToString() + " goals" + retornoAno);
                }

                // team2
                if (!string.IsNullOrEmpty(team2.Trim()))
                {
                    var team2Results = responseJson.data.GroupBy(x => x.team2).ToList();
                    foreach (var result in team2Results)
                    {
                        foreach (var goal in result.Select(x => x.team2goals))
                        {
                            team2goals += int.Parse(goal);
                        }
                    }
                    Console.WriteLine("Team " + team2 + " scored " + team2goals.ToString() + " goals" + retornoAno);
                }

                // team1 e team2
                if (string.IsNullOrEmpty(team1.Trim()) && string.IsNullOrEmpty(team2.Trim()))
                {
                    foreach (var match in responseJson.data)
                    {
                        retornoAno = " in " + match.year;
                        Console.WriteLine("Team " + match.team1 + " scored " + match.team1goals.ToString() + " goals " + retornoAno);
                        Console.WriteLine("Team " + match.team2 + " scored " + match.team2goals.ToString() + " goals " + retornoAno);
                    }
                }
            }

        }
        return new footballmatches();
    }
}