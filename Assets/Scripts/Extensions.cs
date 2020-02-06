using System.Collections.Generic;

public static class ListExtensions {
	public static int Last(this List<int> list){
		return list[list.Count - 1];
	}

	public static List<int> Last(this List<List<int>> list){
		return list[list.Count - 1];
	}
}