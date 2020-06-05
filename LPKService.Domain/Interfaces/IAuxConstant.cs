using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPKService.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс класса AuxConstant
    /// </summary>
    public interface IAuxConstant
    {
        int GetIntAuxConstant(string constId);
        string GetStringAuxConstant(string constId);
        float GetFloatAuxConstant(string constId);
    }
}
