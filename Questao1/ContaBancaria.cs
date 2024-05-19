namespace Questao1
{
    class ContaBancaria
    {
        public ContaBancaria(int numero, string titular)
        {
            NumeroConta = numero;
            Titular = titular;
            Saldo = 0;
            Retorno = $"Conta {NumeroConta}, Titular: {Titular}, Saldo: ${Saldo}";
        }

        public ContaBancaria(int numero, string titular, double depositoInicial)
        {
            NumeroConta = numero;
            Titular = titular;
            Saldo = depositoInicial;
            Retorno =  $"Conta {NumeroConta}, Titular: {Titular}, Saldo: ${Saldo}";
        }


        private protected int NumeroConta { get; set; }
        public string Titular { get; set; }
        private protected double Saldo { get; set; }
        private protected double TaxaBancaria { get; set; } = 3.50;
        public string Retorno { get; set; }


        public void Deposito(double quantia)
        {
            Saldo += quantia;
            Retorno = $"Conta {NumeroConta}, Titular: {Titular}, Saldo: ${Saldo}";
        }

        public void Saque(double quantia)
        {
            Saldo -= quantia;
            CobrarTaxa();
            Retorno = $"Conta {NumeroConta}, Titular: {Titular}, Saldo: ${Saldo}";
        }

        private void CobrarTaxa()
        {
            Saldo -= TaxaBancaria;
        }
    }
}
