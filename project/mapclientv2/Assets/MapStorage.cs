using System;
using System.IO;
using System.Collections.Generic;

public class MapStorage
{
	private int mapCellCapacity = 100;
	private Map map;
	private List<MapCell> mapCells;
	
	public MapStorage (Map map)
	{
		this.map = map;
	}
	
	public void Init ()
	{
		mapCells = new List<MapCell> (map.mapCells);
		
		for (int i=0; i < mapCells.Count; i++) {
			MapCell mapCell = mapCells [i];
			String filePath = generatePath (mapCell);
			if (File.Exists (filePath)) {
				ReadMapCellFromFile (mapCell, filePath);
			}
		}
		
		SortMapCells();
	}
	
	private String generatePath (MapCell mapCell)
	{
		return ".\\" + mapCell.cellX + "_" + mapCell.cellY;
	}
	
	public void ReadMapCellFromFile (MapCell mapCell, String filePath)
	{
		FileStream fileStream = new FileStream (filePath, FileMode.Open);
		BinaryReader br = new BinaryReader (fileStream);
		
		int left = br.ReadInt32 ();
		int top = br.ReadInt32 ();
		int cellWidth = br.ReadInt32 ();
		int cellHeight = br.ReadInt32 ();
		int lifeTime = br.ReadInt32 ();
		int hitCount = br.ReadInt32 ();
			
		for (int row=0; row < cellHeight; row++) {
			ReadIntArray (br, map.data, (top + row) * map.width + left, cellWidth);	
		}
			
		mapCell.DataReady ();
		mapCell.lifeTime = lifeTime;	
		mapCell.hitCount = hitCount;
		
		br.Close ();
		fileStream.Close ();
	}
	
	private Boolean isStorageAvailable (MapCell mapCell)
	{
		int index = mapCells.IndexOf(mapCell);
		return index >=0 && index < mapCellCapacity;
	}
	
	private void SortMapCells ()
	{
		mapCells.Sort ((mapCell1, mapCell2) => CompareMapCell(mapCell1, mapCell2));
	}
	
	private int CompareMapCell(MapCell mapCell1, MapCell mapCell2)
	{
		int hitCount1 = mapCell1.hitCount;
		int hitCount2 = mapCell2.hitCount;
		
		if(hitCount1 == hitCount2)
		{
			return 0;
		}
		else if(hitCount1 < hitCount2)
		{
			return 1;
		}
		else
		{
			return -1;
		}
	}
	
	public void WriteMapCellsToFiles (MapCell[] mapCells)
	{
		SortMapCells ();
		
		for (int i=0; i < mapCells.Length; i++) {
			MapCell mapCell = mapCells [i];
			if (isStorageAvailable (mapCell)) {
				WriteMapCellToFile (mapCell, mapCell.cellX + "_" + mapCell.cellY);
			}
		}
	}
	
	private void WriteMapCellToFile (MapCell mapCell, String filePath)
	{
		FileStream fileStream = new FileStream (filePath, FileMode.Create);
		BinaryWriter bw = new BinaryWriter (fileStream);
		
		bw.Write (mapCell.bound.left);
		bw.Write (mapCell.bound.top);
		bw.Write (mapCell.bound.width);
		bw.Write (mapCell.bound.height);
		bw.Write (mapCell.lifeTime);
		bw.Write (mapCell.hitCount);
			
		for (int row=mapCell.bound.top; row < mapCell.bound.getBottom(); row++) {
			WriteIntArray (bw, map.data, row * map.width + mapCell.bound.left, mapCell.bound.width);
		}
		
		bw.Flush ();
		bw.Close ();
		
		fileStream.Close ();
	}
	
	private void WriteIntArray (BinaryWriter writer, int[] array, int offset, int count)
	{
		for (int i=0; i < count; i++) {
			writer.Write (array [offset + i]);
		}
	}
	
	private void ReadIntArray (BinaryReader reader, int[] array, int offset, int count)
	{
		for (int i=0; i < count; i++) {
			array [offset + i] = reader.ReadInt32 ();
		}
	}
}