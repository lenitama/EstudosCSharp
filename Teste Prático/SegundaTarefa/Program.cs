using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var searchTerms = new Dictionary<string, object>

            {

                {"Name", "João"},
                {"Phone.Number", "3234-2343"},
                {"Phone.Ddd.Code", 32}

            };



            // aqui, você pode construir na mão um List<Contact>(). Adicione ítens nele manualmente

            var dataSource = GetDataSource();



            // essa é a função que vc tem que fazer

            var searchExpression = GetFilterExpression<Contact>(searchTerms);



            // searchResult só pode conter contatos cujo o nome seja "João" e o telefone seja "3234-2343"
            IQueryable<Contact> queryableData = dataSource.AsQueryable<Contact>();
            var searchResult = queryableData.Where(searchExpression).ToList();
            foreach (Contact c in searchResult)
            {
                Console.WriteLine("Name:" + c.Name + " Phone: " + c.Phone.Ddd.Code + " " + c.Phone.Number + " Date Of Birth: " + c.DateOfBirth);
            }
            Console.ReadKey();
        }




        private static Expression<Func<T, bool>> GetFilterExpression<T>(Dictionary<string, object> searchTerms)
        {

            // the magic goes here.

            //Cria o parâmetro para a expressão
            ParameterExpression pe = Expression.Parameter(typeof(T), "x");

            // Cria as expressões 'x.nome_campo == valor_campo' para cada campo no Dicionário
            Expression principal = null;
            foreach (KeyValuePair<string, object> entry in searchTerms)
            {
                Expression left = null;

                //cria propriedades de tipos hierarquicos
                if (entry.Key.Contains('.'))
                {
                    string[] campos = entry.Key.Split('.');
                    Type t = null;
                    for (int i = 0; i < campos.Length; i++)
                    {
                        string campo = campos[i];
                        if (left == null)
                        {
                            left = Expression.Property(pe, typeof(T).GetProperty(campo));
                            t = typeof(T).GetProperty(campo).PropertyType;
                        }
                        else
                        {
                            left = Expression.Property(left, t.GetProperty(campo));
                            t = t.GetProperty(campo).PropertyType;
                        }
                    }
                }
                //cria propriedades de tipos simples
                else
                {
                    left = Expression.Property(pe, typeof(T).GetProperty(entry.Key));

                }

                //cria o valor a ser comparado com o campo
                Expression right = Expression.Constant(entry.Value);

                //inclui a expressão 'x.left == right' na árvore de expressões
                if (principal == null)
                {
                    //se é a primeira expressão coloca na raiz da árvore
                    principal = Expression.Equal(left, right);
                }
                else
                {
                    //se não, concatena usando o operador AND
                    Expression e = Expression.Equal(left, right);
                    principal = Expression.And(principal, e);
                }
            }
            //cria a expresão lambda usando a árvore de expressões e o parametro definido anteriormente
            return Expression.Lambda<Func<T, bool>>(principal, new ParameterExpression[] { pe });

        }

        private static List<Contact> GetDataSource()
        {
            var contacts = new List<Contact>();
            contacts.Add(new Contact { Name = "Lenita", DateOfBirth = new DateTime(1990, 8, 30), Phone = new Phone { Number = "3221-7347", Ddd = new Ddd { Code = 32 } } });
            contacts.Add(new Contact { Name = "João", DateOfBirth = new DateTime(1988, 7, 10), Phone = new Phone { Number = "3234-2343", Ddd = new Ddd { Code = 32 } } });
            contacts.Add(new Contact { Name = "Maria", DateOfBirth = new DateTime(1975, 3, 22), Phone = new Phone { Number = "3221-3164", Ddd = new Ddd { Code = 32 } } });
            contacts.Add(new Contact { Name = "José", DateOfBirth = new DateTime(1986, 4, 4), Phone = new Phone { Number = "3223-5752", Ddd = new Ddd { Code = 32 } } });
            contacts.Add(new Contact { Name = "João", DateOfBirth = new DateTime(1997, 1, 30), Phone = new Phone { Number = "3234-2343", Ddd = new Ddd { Code = 21 } } });

            return contacts;
        }
    }
}
