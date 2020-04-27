using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Aq.ExpressionJsonSerializer.Test
{
    [TestClass]
    public class ExpressionJsonSerializerTest
    {
        [TestMethod]
        public void Assignment()
        {
            TestExpression((Expression<Func<Context, int>>)(c => c.A + c.B));
        }

        [TestMethod]
        public void BitwiseAnd()
        {
            TestExpression((Expression<Func<Context, int>>)(c => c.A & c.B));
        }

        [TestMethod]
        public void LogicalAnd()
        {
            TestExpression((Expression<Func<Context, bool>>)(c => c.A > 0 && c.B > 0));
        }

        [TestMethod]
        public void ArrayIndex()
        {
            TestExpression((Expression<Func<Context, int>>)(c => c.Array[0]));
        }

        [TestMethod]
        public void ArrayLength()
        {
            TestExpression((Expression<Func<Context, int>>)(c => c.Array.Length));
        }

        [TestMethod]
        public void Method()
        {
            TestExpression((Expression<Func<Context, int>>)(c => c.Method()));
        }

        [TestMethod]
        public void MethodWithArguments()
        {
            TestExpression((Expression<Func<Context, int>>)(c => c.Method("B")));
        }

        [TestMethod]
        public void Coalesce()
        {
            TestExpression((Expression<Func<Context, int>>)(c => c.C ?? c.A));
        }

        [TestMethod]
        public void Conditional()
        {
            TestExpression((Expression<Func<Context, int>>)(c => c.C == null ? c.A : c.B));
        }

        [TestMethod]
        public void Convert()
        {
            TestExpression((Expression<Func<Context, int>>)(c => (short)(c.C ?? 0)));
        }

        [TestMethod]
        public void Decrement()
        {
            TestExpression((Expression<Func<Context, int>>)(c => c.A - 1));
        }

        [TestMethod]
        public void DivisionWithCast()
        {
            TestExpression((Expression<Func<Context, float>>)(c => (float)c.A / c.B));
        }

        [TestMethod]
        public void Equality()
        {
            TestExpression((Expression<Func<Context, bool>>)(c => c.A == c.B));
        }

        [TestMethod]
        public void Xor()
        {
            TestExpression((Expression<Func<Context, int>>)(c => c.A ^ c.B));
        }

        [TestMethod]
        public void LinqExtensions()
        {
            TestExpression((Expression<Func<Context, int>>)(c => c.Array.FirstOrDefault()));
        }

        [TestMethod]
        public void GreaterThan()
        {
            TestExpression((Expression<Func<Context, bool>>)(c => c.A > c.B));
        }

        [TestMethod]
        public void Increment()
        {
            TestExpression((Expression<Func<Context, int>>)(c => c.A + 1));
        }

        [TestMethod]
        public void Indexer()
        {
            TestExpression((Expression<Func<Context, int>>)(c => c["A"]));
        }

        [TestMethod]
        public void Invoke()
        {
            TestExpression((Expression<Func<Context, int>>)(c => c.Func()));
        }

        [TestMethod]
        public void Constant()
        {
            TestExpression((Expression<Func<Context, bool>>)(c => false));
        }

        [TestMethod]
        public void Lambda()
        {
            TestExpression((Expression<Func<Context, int>>)(c => ((Func<Context, int>)(_ => _.A))(c)));
        }

        [TestMethod]
        public void LeftShift()
        {
            TestExpression((Expression<Func<Context, int>>)(c => c.A << c.C ?? 0));
        }

        [TestMethod]
        public void PropertyAccess()
        {
            TestExpression((Expression<Func<Context, int>>)(c => c.B));
        }

        [TestMethod]
        public void Negation()
        {
            TestExpression((Expression<Func<Context, int>>)(c => -c.A));
        }

        [TestMethod]
        public void New()
        {
            TestExpression((Expression<Func<Context, object>>)(c => new object()));
        }

        [TestMethod]
        public void NewWithArguments()
        {
            TestExpression((Expression<Func<Context, object>>)(c => new String('s', 1)));
        }

        [TestMethod]
        public void InitArray()
        {
            TestExpression((Expression<Func<Context, int[]>>)(c => new[] { 0 }));
        }

        [TestMethod]
        public void InitEmptyArray()
        {
            TestExpression((Expression<Func<Context, int[,]>>)(c => new int[3, 2]));
        }

        [TestMethod]
        public void TypeAs()
        {
            TestExpression((Expression<Func<Context, object>>)(c => c as object));
        }

        [TestMethod]
        public void TypeOf()
        {
            TestExpression((Expression<Func<Context, bool>>)(c => typeof(Context) == c.GetType()));
        }

        [TestMethod]
        public void TypeIs()
        {
            TestExpression((Expression<Func<Context, bool>>)(c => c is object));
        }

        [TestMethod]
        public void MethodResultCast()
        {
            TestExpression((Expression<Func<Context, int>>)(c => (int)c.Method3()));
        }

        private sealed class Context
        {
            public int A;
            public int B { get; set; }
            public int? C;
            public int[] Array;
            public int this[string key]
            {
                get
                {
                    switch (key)
                    {
                        case "A": return this.A;
                        case "B": return this.B;
                        case "C": return this.C ?? 0;
                        default: throw new NotImplementedException();
                    }
                }
            }
            public Func<int> Func;
            public int Method() { return this.A; }
            public int Method(string key) { return this[key]; }
            public object Method3() { return this.A; }
        }

        private static JsonSerializerSettings CreateSerializerSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            return settings;
        }

        private static void TestExpression(LambdaExpression source)
        {
            var random = new Random();
            int u;
            var context = new Context
            {
                A = random.Next(),
                B = random.Next(),
                C = (u = random.Next(0, 2)) == 0 ? null : (int?)u,
                Array = new[] { random.Next() },
                Func = () => u
            };

            var settings = CreateSerializerSettings();
            settings.Converters.Add(new ExpressionJsonConverter(
                Assembly.GetAssembly(typeof(ExpressionJsonSerializerTest))
            ));

            var json = JsonConvert.SerializeObject(source, settings);
            var target = JsonConvert.DeserializeObject<LambdaExpression>(json, settings);

            Assert.AreEqual(
                ExpressionResult(source, context),
                ExpressionResult(target, context)
            );
        }

        private static string ExpressionResult(LambdaExpression expr, Context context)
        {
            return JsonConvert.SerializeObject(expr.Compile().DynamicInvoke(context), CreateSerializerSettings());
        }
    }
}
