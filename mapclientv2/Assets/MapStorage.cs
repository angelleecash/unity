using System;
using System.IO;
using System.Collections.Generic;

public class MapStorage
{
	public int capacity = 100;
	
	public MapStorage ()
	{
	}
	
	public void load(Map map)
	{
		FileStream fileStream = new FileStream(".\\map.data", FileMode.Open);
		BinaryReader br = new BinaryReader(fileStream);
		int mapCellCount = br.ReadInt32();
		mapCellCount = Math.Min(mapCellCount, capacity);
		for(int i=0 ;i < mapCellCount; i++)
		{
			int cellX = br.ReadInt32();
			int cellY = br.ReadInt32();
			int left = br.ReadInt32();
			int top = br.ReadInt32();
			int cellWidth = br.ReadInt32();
			int cellHeight = br.ReadInt32();
			int lifeTime = br.ReadInt32();
			
			for(int row=0; row < cellHeight; row++)
			{
				ReadIntArray(br, map.data, (top+row)*map.width + left, cellWidth);	
			}
			
			MapCell mapCell = map.getMapCellByCellPosition(cellX, cellY);
			mapCell.dataReady();
			mapCell.lifeTime = lifeTime;
			
			
		}
		
		br.Close();
		fileStream.Close();
	}
	
	public void save(Map map)
	{
		FileStream fileStream = new FileStream(".\\map.data", FileMode.Create);
		BinaryWriter bw = new BinaryWriter(fileStream);
		
		List<MapCell> mapCells = map.mapCells;
		mapCells.Sort((mapCell1, mapCell2) => mapCell1.hitCount - mapCell2.hitCount);
		
		int mapCellCount = Math.Min (capacity, mapCells.Count);
		bw.Write(mapCellCount);
		
		for(int i=0; i < mapCellCount; i++)
		{
			MapCell mapCell = mapCells[i];
			bw.Write(mapCell.cellX);
			bw.Write(mapCell.cellY);
			bw.Write(mapCell.bound.left);
			bw.Write(mapCell.bound.top);
			bw.Write(mapCell.bound.width);
			bw.Write(mapCell.bound.height);
			bw.Write(mapCell.lifeTime);
			
			for(int row=mapCell.bound.top; row < mapCell.bound.getBottom(); row++)
			{
				WriteIntArray(bw, map.data, row*map.width+mapCell.bound.left, mapCell.bound.width);
			}
		}
		
		bw.Flush();
		bw.Close();
		
		fileStream.Close();
		
	}
	
	private void WriteIntArray(BinaryWriter writer, int[] array, int offset, int count)
	{
		for(int i=0;i < count; i++)
		{
			writer.Write(array[offset + i]);
		}
	}
	
	private void ReadIntArray(BinaryReader reader, int[] array, int offset, int count)
	{
		for(int i=0;i < count; i++)
		{
			array[offset + i] = reader.ReadInt32();
		}
	}
}