using System;
using System.Globalization;

namespace Questao1
{
    class ContaBancaria {

        public int _numero { get; set; }
        public string _titular { get; set; }
        public double? _quantia { get; set; }          

        public ContaBancaria(int numero, string titular, double? quantia = 0)
        {
            this._numero = numero;
            this._titular = titular;
            this._quantia = quantia;
        }

        public void Deposito(double? quantia)
        {
            _quantia += quantia;
        }

        public void Saque(double? quantia)
        {
            _quantia = (_quantia - quantia) - 3.50;
        }

        public string DadosConta()
        {
            var resp = $"Conta {_numero}, Titular: {_titular}, Saldo: ${_quantia}";

            return resp;
        }
    }
}
