using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Previsao
{
    class Program
    {
        
        static string Url = "http://api.ipma.pt/open-data/forecast/meteorology/cities/daily/hp-daily-forecast-day";
        static string UrlReg = "http://api.ipma.pt/open-data/distrits-islands.json";
        static void Main(string[] args)
        {
            int dia, reg = 0;
            char tp;

            Console.WriteLine("O que pretende saber?");
            Volta:
            Console.WriteLine("Temperatura » digite t\n" +
                "Precipitação » digite p\n" + 
                "Região » digite r");

            tp = char.Parse(Console.ReadLine());
            if (tp == 't')
                Console.WriteLine("Escolheu Temperatura.\n");
            else if (tp == 'p')
                Console.WriteLine("Escolheu Precipitação.\n");
            else if (tp == 'r')
            {
                Volta2:
                Console.WriteLine("Escolheu Região.\n" +
                    "Escolha a Região pretendida:\n" +
                    "1 Continente, 2 Arq.Madeira, 3 Arq.Açores");
                reg = int.Parse(Console.ReadLine());
                if (reg < 1 || reg > 3)
                {
                    Console.WriteLine("Valor errado!");
                    goto Volta2;
                }
            }
                
            else
            {
                Console.WriteLine("Escolha errada!");
                goto Volta;
            }


            Volta1:
            Console.WriteLine("Digite 0 para o dia de hoje, 1 para amanhã e 2 para depois de amanhã.");
            dia = int.Parse(Console.ReadLine());
            if (dia == 0)
                Console.WriteLine("Escolheu o dia de hoje.");
            else if (dia == 1)
                Console.WriteLine("Escolheu o dia de amanhã.");
            else if (dia == 2)
                Console.WriteLine("Escolheu o dia depois de amanhã.");
            else 
            {
                Console.WriteLine("Valor errado!");
                goto Volta1;
            }
                

            Url += dia.ToString() + ".json";
           
            GetData(tp, reg);

            
            Console.Read();
        }




        static async void GetData(char tp, int reg)
        {
            if (tp == 't')
            {
                HttpClient client = new HttpClient();

                var content = await client.GetStringAsync(Url);
                var result = JsonConvert.DeserializeObject<Ipma>(content);

                Console.WriteLine(result.country);
                Console.WriteLine("Os dados metereologicos do dia {0} são:", result.forecastDate);

                float Count = 0, Temp = 0;
                float Med;
                foreach (Data data in result.data)
                {
                    Count++;

                    if (tp == 't')
                    {
                        Console.WriteLine("Temperatura máxima é {0}ºC",data.tMax);
                        Console.WriteLine("Temperatura mínima é {0}ºC",data.tMin);
                        Temp = Temp + data.tMax + data.tMin;
                    }


                }
                Console.WriteLine("São {0} cidades.", Count); 
                if (tp == 't')
                {
                    Med = Temp / (Count * 2);
                    Console.WriteLine("Temperatura média do país é de: {0}ºC.", Med);
                }
            }

            else
            {
                HttpClient client = new HttpClient();

                var content = await client.GetStringAsync(Url);
                var result = JsonConvert.DeserializeObject<Ipma>(content);
                var contentreg = await client.GetStringAsync(UrlReg);
                var resultreg = JsonConvert.DeserializeObject<IpmaReg>(contentreg);

                Console.WriteLine(result.country);
                Console.WriteLine("Os dados metereológicos do dia {0} são:", result.forecastDate);


                //List<string> distrito = new List<string>();
                //List<double> id = new List<double>();

                if (tp == 'r')
                {


                    foreach (DataReg data in resultreg.data)
                    {
                        if (data.idRegiao == reg)
                        {
                            distrito.Add(data.local);

                            id.Add(data.globalIdLocal);


                        }
                    }

                    foreach (Data data in result.data)
                    {
                        if (id.Contains(data.globalIdLocal))
                        {
                            int index = id.IndexOf(data.globalIdLocal);
                            Console.WriteLine(distrito[index]);
                            Console.WriteLine("Temperatura máxima: {0}ºC", data.tMax);
                            Console.WriteLine("Temperatura mínima: {0}ºC", data.tMin);
                            Console.WriteLine("Possiblidade de Precipitação: {0}%", data.precipitaProb);
                            Console.WriteLine("Direção do vento: {0} \n", data.predWindDir);
                        }

                    }
                }
                else if (tp == 'p')
                {
                    
                    foreach (DataReg data in resultreg.data)
                    {
                        
                        distrito.Add(data.local);
                    }
                    int index = 0;
                    foreach (Data data in result.data)
                    {
                        Console.WriteLine("Probabilidade de chuva em {0} é de {1}%.", distrito[index], data.precipitaProb);
                        index++;
                    }
                }
            }
        }
    }
}
