// See https://aka.ms/new-console-template for more information
using Questao2;
using System.Net.Http.Headers;
using System.Text.Json;


string teamName = "Paris Saint-Germain";
int year = 2013;
int totalGoals = await getTotalScoredGoalsAsync(teamName, year);

Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

teamName = "Chelsea";
year = 2014;
totalGoals = await getTotalScoredGoalsAsync(teamName, year);

Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

// Output expected:
// Team Paris Saint - Germain scored 109 goals in 2013
// Team Chelsea scored 92 goals in 2014

static async Task<int> getTotalScoredGoalsAsync(string team, int year)
{
    using (var client = new HttpClient())
    {
        client.BaseAddress = new System.Uri("https://jsonmock.hackerrank.com/");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        HttpResponseMessage responseTeam1 = new HttpResponseMessage();
        HttpResponseMessage responseTeam2 = new HttpResponseMessage();
        List<Times> times = new List<Times>();

        int contGol = 0;        

        var res1 = await client.GetAsync($"api/football_matches?year={year}&team1={team}&page=1");
        var res2 = await client.GetAsync($"api/football_matches?year={year}&team2={team}&page=1");

        if (res1.IsSuccessStatusCode || res2.IsSuccessStatusCode)
        {  
            var resultTeam1 = JsonSerializer.Deserialize<Times>(await res1.Content.ReadAsStringAsync(), new JsonSerializerOptions(){PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
            var resultTeam2 = JsonSerializer.Deserialize<Times>(await res2.Content.ReadAsStringAsync(), new JsonSerializerOptions(){PropertyNamingPolicy = JsonNamingPolicy.CamelCase});


            for (int i = 1; i <= resultTeam1.total_pages; i++)
            {
                responseTeam1 = await client.GetAsync($"api/football_matches?year={year}&team1={team}&page={i}");
                times.Add(JsonSerializer.Deserialize<Times>(await responseTeam1.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
                                
            }

            for (int i = 1; i <= resultTeam2.total_pages; i++)
            {
                responseTeam2 = await client.GetAsync($"api/football_matches?year={year}&team2={team}&page={i}");
                times.Add(JsonSerializer.Deserialize<Times>(await responseTeam2.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));

            }

            foreach (var lista in times)
            {
                foreach (var item2 in lista.data.Where(x => x.team1 == team))
                {
                    contGol += item2.team1goals != null ? int.Parse(item2.team1goals) : 0;
                }

                foreach (var item2 in lista.data.Where(x => x.team2 == team))
                {
                    contGol += item2.team2goals != null ? int.Parse(item2.team2goals) : 0;
                }
            }

                       

        }

        return contGol;

    }

}
