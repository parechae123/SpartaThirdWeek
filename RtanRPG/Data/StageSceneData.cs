using RtanRPG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.Data
{
    public class StageSceneData : SceneData
    {
        public int[][] map { get; set; }
        public SpawnData[] Positions { get; set; }
    }
}
