using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreationModelPlugin
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class CreationModel:IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet element) 
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            List<Level> listlevel = new FilteredElementCollector(doc)
                .OfClass(typeof(Level))
                .OfType<Level>()
                .ToList();
            Level level1 = listlevel
               .Where(x=>x.Name.Equals("Уровень 1"))
               .FirstOrDefault();

            Level level2 = listlevel
               .Where(x => x.Name.Equals("Уровень 2"))
               .FirstOrDefault();

            List<XYZ> points = new List<XYZ>();
            List<Wall> walls = new List<Wall>();
            double widht = UnitUtils.ConvertToInternalUnits(10000, UnitTypeId.Millimeters);
            double depth = UnitUtils.ConvertToInternalUnits(5000, UnitTypeId.Millimeters);
            double dx = widht / 2;
            double dy = depth / 2;
            points.Add(new XYZ(-dx, -dy, 0));
            points.Add(new XYZ(dx, -dy, 0));
            points.Add(new XYZ(dx, dy, 0));
            points.Add(new XYZ(-dx, dy, 0));
            points.Add(new XYZ(-dx, -dy, 0));


            Transaction transaction = new Transaction(doc, "Построение стен");
            transaction.Start();
            for (int i = 0; i < 4; i++)

            {
                Wall wall1 = CreateWall(doc, level1);
                Wall wall2 = CreateWall(doc, level2);
                //Line line = Line.CreateBound(points[i], points[i + 1]);
                //Wall walls= CreateWall(doc, Level)
                //walls.Add(wall);
                //wall.get_Parameter(BuiltInParameter.WALL_HEIGHT_TYPE).Set(level.Id);
            }
                
            transaction.Commit();

            return Result.Succeeded;
        }
    }
    //public class LevelsUtils
    //{
    //    public static List<Level> GetLevels(ExternalCommandData commandData)
    //    {
    //        var doc = commandData.Application.ActiveUIDocument.Document;
    //        List<Level> levels = new FilteredElementCollector(doc)
    //                                                 .OfClass(typeof(Level))
    //                                                 .Cast<Level>()
    //                                                 .ToList();
    //        return levels;
    //    }
    //}
    public static Wall CreateWall(Autodesk.Revit.DB.Document document, Level level)
    {
        List<XYZ> points = new List<XYZ>();
        List<Wall> walls = new List<Wall>();
        double widht = UnitUtils.ConvertToInternalUnits(10000, UnitTypeId.Millimeters);
        double depth = UnitUtils.ConvertToInternalUnits(5000, UnitTypeId.Millimeters);
        double dx = widht / 2;
        double dy = depth / 2;
        points.Add(new XYZ(-dx, -dy, 0));
        points.Add(new XYZ(dx, -dy, 0));
        points.Add(new XYZ(dx, dy, 0));
        points.Add(new XYZ(-dx, dy, 0));
        points.Add(new XYZ(-dx, -dy, 0));
 
            for (int i = 0; i < 4; i++)
            {
                Line line = Line.CreateBound(points[i], points[i + 1]);
                Wall wall = Wall.Create(document, line, level.Id, false);
                walls.Add(wall);
                wall.get_Parameter(BuiltInParameter.WALL_HEIGHT_TYPE).Set(level.Id);
            }
        return walls;



    }
}
