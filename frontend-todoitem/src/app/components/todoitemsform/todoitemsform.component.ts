import { Component, OnInit } from '@angular/core';
import { ToDoItem } from 'src/app/models/todoitem';
import { ToDoItemService } from 'src/app/services/todoitem.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-todoitemsform',
  templateUrl: './todoitemsform.component.html',
  styleUrls: ['./todoitemsform.component.css']
})

export class ToDoItemFormComponent implements OnInit {
  allToDoItems: Array<ToDoItem>;
  filteredToDoItems: Array<ToDoItem>;
  filter: string;
  isDoneButtonVisible: boolean;
  constructor(private toDoItemService: ToDoItemService, private router: Router) { }

  ngOnInit(): void {
    console.log('to do list is initiated');
    this.reload();

  }

  reload(): void {
    console.log('to do list is reloaded from database');
    this.getAllToDoItems();
    this.filter = '';
    this.isDoneButtonVisible = true;
  }

  getAllToDoItems(): void{
    console.log("load all to do items from database");
    this.toDoItemService.getToDoItems().subscribe( items => { this.allToDoItems = items; this.filteredToDoItems = items;});
  }

  updateSearchResult(): void{
    if (this.filter === '')
    {
      this.filteredToDoItems = this.allToDoItems;
    }
    else
    {
      this.filteredToDoItems = this.allToDoItems.filter(item => item.description.toLowerCase().includes(this.filter.toLowerCase()));
    }
  }

  itemDoneChanged(item: ToDoItem): void{
    item.done = !item.done;
    this.updateItemInDatabase(item, true);
    //this.getAllToDoItems();
    this.updateSearchResult();
  }

  itemFavoriteChanged(item: ToDoItem): void{
    item.favorite = !item.favorite;
    this.updateItemInDatabase(item, false);
    //this.getAllToDoItems();
    this.updateSearchResult();
  }

  private updateItemInDatabase(updateItem: ToDoItem, doneChanged: boolean): void{
    if (doneChanged && updateItem.children.length > 0)
    {
      if (updateItem.done === true){
        updateItem.children.forEach(i => i.done = true);
      }
      else
      {
        updateItem.children.forEach(i => i.done = false);
      }
      if (updateItem.children.every(i => i.done === true)){
        updateItem.done = true;
      }
    }
    this.toDoItemService.upsertToDoItem(updateItem).subscribe();
  }

  sortByDescription(): void{
    // here assume that the two filters can be applied together. If this is not required, need to update the filterTODoItem before sorting.
    this.filteredToDoItems = this.filteredToDoItems.sort((a, b) => (b.description > a.description ? -1 : 1));
  }

  sortByCreationTime(): void{
    // here assume that the two filters can be applied together. If this is not required, need to update the filterTODoItem before sorting.
    this.filteredToDoItems = this.filteredToDoItems.sort((a, b) => new Date(a.createdTime).getTime() - new Date(b.createdTime).getTime());
  }

  createItem(): void{
    this.router.navigate(['item']);
  }
}
