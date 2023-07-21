using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationClinik.Infras
{
    public class LambdaCommand : Command
    {
        private readonly Action<object> _Execate;
        private readonly Func<object, bool> _CanExecate;
        public LambdaCommand(Action<object> Execate, Func<object, bool> CanExecate = null)
        {
            _Execate = Execate ?? throw new ArgumentNullException(nameof(Execate));
            _CanExecate = CanExecate;
        }

        public override bool CanExecute(object? parameter) => _CanExecate?.Invoke(parameter) ?? true;

        public override void Execute(object? parameter) => _Execate(parameter);
    }
}
