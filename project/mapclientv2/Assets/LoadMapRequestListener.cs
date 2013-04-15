using System;

public interface LoadMapRequestListener
{
	void MapCellsLoaded(MapCell[] mapCells);
	void MapCellsLoadFail();
}
