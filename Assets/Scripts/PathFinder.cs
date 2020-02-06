using System.Collections.Generic;

public static class PathFinder
{
   public static List<int> FindPath(int from, List<bool> field){

	var path = new List<List<int>> ();
	List<List<int>> newPath;

    //задаем начальную точку
	path.Add(new List<int>());
	path.Last().Add (from);

	var iter = 0;
    do {
		var addNewPointToPath = false;
		var reachFinal = false;
		iter++;
		if (iter>FieldController.MaxCell) break;
		newPath = new List<List<int>>();

		//проходимся по всем путям, получаем последнюю точку и проверяем пути, куда можно передвинутся
		foreach (var item in path) {
			//последняя точка
			var id = item.Last();

			//получаем возможные пути движения из этой точки
			var nearCells = GetNearCellsForMove (id);
			foreach (var nc in nearCells) {
				if (!field[nc-1]) {
					if (nc == FieldController.MaxCell)
						reachFinal = true;

					//если новой ячейки нет с пройденом пути
					if (!item.Contains(nc)){
						addNewPointToPath = true;
						//сделано так что бы копировать новый список а не ссылку на существующий
						newPath.Add(new List<int> (item));
						newPath.Last().Add(nc);
					}
				}
			}
		}
		path = newPath;
		if (!addNewPointToPath || reachFinal) break;
	} while(true);


	//находим количество ячеек в самом коротком пути, который приведет к финишу
	var minLenght = int.MaxValue;
	foreach (var item in path) {
		if (item.Last () == FieldController.MaxCell) {
			if (item.Count < minLenght)
				minLenght = item.Count;
		}
	}

	//если пути нет
	if (minLenght == int.MaxValue) {
		return null;
	}

	//находим все пути с минимальной длинной
	var paths = new List<List<int>>();
	foreach (var item in path) {
		if (item.Last() == FieldController.MaxCell) {
			if (item.Count == minLenght) {
				paths.Add (item);
			}
		}
	}
    return paths [0];
   }

   private static List<int> GetNearCellsForMove(int id)
   {
        var cells = new List<int>();
        //right
        if (id % 10 != 0)
        {
            cells.Add(id + 1);
        }

        //left
        if ((id-1) % 10 != 0)
        {
            cells.Add(id - 1);
        }

        //top
        if (id<=90)
        {
            cells.Add(id+10);
        }

        //bottom
        if (id>10)
        {
            cells.Add(id-10);
        }

        return cells;
   }
}