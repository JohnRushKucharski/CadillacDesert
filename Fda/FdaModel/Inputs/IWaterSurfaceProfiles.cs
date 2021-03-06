﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Inputs
{
    public interface IWaterSurfaceProfiles: IValidateData
    {
        IDictionary<float, Statistics.CurveIncreasing> Profiles { get; }
    }
}
