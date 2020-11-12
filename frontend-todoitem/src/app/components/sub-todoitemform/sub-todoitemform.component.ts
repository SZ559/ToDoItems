import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToDoItem } from 'src/app/models/todoitem';
import { ToDoItemService } from 'src/app/services/todoitem.service';
import {MatDialog} from '@angular/material/dialog';
import { DialogComponent } from '../dialog/dialog.component';

@Component({
  selector: 'app-sub-todoitemform',
  templateUrl: './sub-todoitemform.component.html',
  styleUrls: ['./sub-todoitemform.component.css']
})
export class SubToDoItemFormComponent implements OnInit {
  constructor(private toDoItemService: ToDoItemService, private router: Router, private activatedRoute: ActivatedRoute,
              private dialog: MatDialog) { }

  item: ToDoItem;
  ngOnInit(): void {
    this.item = {description: '', done: false, favorite: false, children: null};
    const id = this.activatedRoute.snapshot.params.id;
    if (id !== '' && id !== undefined){
      this.getToDoItem(id);
    }
  }

  getToDoItem(id: string): void{
    console.log('get to do item from database');
    this.toDoItemService.getToDoItem(id).subscribe( item => { this.item = item; });
  }

  backToToDoItemsList(): void{
    this.openConfirmDialogueForBackToItemsList();
  }

  createSubItem(): void{
    let newSubItem = { description: '', done: false, favorite: false, children: null};
    if (this.item.children == null)
    {
      this.item.children = [newSubItem];
    }
    else
    {
      this.item.children.push(newSubItem);
    }
    this.synchronizeDoneStatusOfMainItem();
  }

  deleteSubItem(subItem: ToDoItem): void{
    const index = this.item.children.indexOf(subItem);
    if (index !== -1){
      this.item.children.splice(index, 1);
      this.synchronizeDoneStatusOfMainItem();
    }
  }

  deleteItem(): void{
    console.log('the item has been deleted:' + this.item.id);
    if (this.item.id !== undefined)
    {
      this.openConfirmDialogForDeleteItem();
    }
    else
    {
      this.openConfirmDialogueForBackToItemsList();
    }
  }

  saveItem(): void{
    this.toDoItemService.upsertToDoItem(this.item).subscribe(result => {
      if(result.id != null)
      {
        this.getToDoItem(result.id);
        this.openConfirmDialogForSaveItem();
        console.log('the item has been saved');
      }
    });
  }

  synchronizeDoneStatusOfSubItems(): void{
    if (this.item.children != null && this.item.children.length > 1)
    {
      if (this.item.done === true){
        this.item.children.forEach(i => i.done = true);
      }
      else
      {
        this.item.children.forEach(i => i.done = false);
      }
    }
  }

  synchronizeDoneStatusOfMainItem(): void{
    if (this.item.children != null && this.item.children.length > 0)
    {
      if (this.item.children.every(i => i.done === true))
      {
        this.item.done = true;
      }
      else
      {
        this.item.done = false;
      }
    }
  }

  openConfirmDialogueForBackToItemsList(): void{
    const dialogRef = this.dialog.open(DialogComponent, {
      width: '250px',
      data: {cancelButton: 'Cancel', confirmButton: 'Discard', content: 'Sure to discard changes?'},
    });
    dialogRef.afterClosed().subscribe(chooseToDiscard =>
      {
        console.log(`Dialog result: ${chooseToDiscard}`);
        if (chooseToDiscard){
          this.navigateBackToToDoItemsList();
        }
    });
  }

  openConfirmDialogForDeleteItem(): void{
    const dialogRef = this.dialog.open(DialogComponent, {
      width: '250px',
      data: {cancelButton: 'Cancel', confirmButton: 'Delete', content: 'Sure to Delete?'},
    });
    dialogRef.afterClosed().subscribe(chooseToDelete =>
      {
        console.log(`Dialog result: ${chooseToDelete}`);
        if (chooseToDelete){
          this.deleteItemFromDatabase();
        }
    });
  }

  openConfirmDialogForSaveItem(): void{
    const dialogRef = this.dialog.open(DialogComponent, {
      width: '250px',
      data: {cancelButton: "Continue Edit", confirmButton: 'Back to To-Do Item List',
      content: 'The To-Do item has been successfully saved!'},
    });
    dialogRef.afterClosed().subscribe(chooseToReturn =>
      {
        console.log(`Dialog result: ${chooseToReturn}`);
        if (chooseToReturn){
          this.navigateBackToToDoItemsList();
        }
    });
  }

  private navigateBackToToDoItemsList(): void{
    this.router.navigate(['/items']);
  }

  private deleteItemFromDatabase(): void{
    this.toDoItemService.deleteToDoItem(this.item.id).subscribe(() => this.navigateBackToToDoItemsList());
  }
}
