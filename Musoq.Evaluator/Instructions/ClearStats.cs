﻿namespace Musoq.Evaluator.Instructions
{
    public class ClearStats : Instruction
    {
        public override void Execute(IVirtualMachine virtualMachine)
        {
            virtualMachine.Current.Stats = new AmendableQueryStats();
            virtualMachine[Register.Ip] += 1;
        }

        public override string DebugInfo()
        {
            return "CLEARING STATS";
        }
    }
}