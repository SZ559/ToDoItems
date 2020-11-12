// import { ToDoItem } from 'src/models/todoitem';
// import { ToDoItemService } from "./todoitem.service";

// let httpClientSpy: { get: jasmine.Spy };
// let heroService: ToDoItemService;

// beforeEach(() => {
//   // TODO: spy on other methods too
//   httpClientSpy = jasmine.createSpyObj('HttpClient', ['get']);
//   heroService = new ToDoItemService(httpClientSpy as any);
// });

// it('should return expected heroes (HttpClient called once)', () => {
//   const expectedHeroes: ToDoItem[] =
//     [];

//   httpClientSpy.get.and.returnValue(expectedHeroes);
//   heroService.getToDoItems().subscribe(
//     heroes => expect(heroes).toEqual(expectedHeroes, 'expected heroes'), fail);
//   expect(httpClientSpy.get.calls.count()).toBe(1, 'one call');
// // })
