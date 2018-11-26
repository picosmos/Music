import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";
import { MusicViewModel } from "../models/MusicViewModel";
import { LibraryService } from "../services/LibraryService";

@Component({
  selector: "library",
  templateUrl: "./Library.web.html",
  styleUrls: ["./Library.web.less"]
})
export class LibraryComponent implements OnInit {
  constructor(
    private _libraryService: LibraryService
  ) {
  }

  public paths: string[] = [];

  public addPath(path: string) {
    if(!path){
      return;
    }
    
    this._libraryService.addPath(path);
    this.paths.push(path);
  }


  public async ngOnInit() {
    this.paths = await this._libraryService.getPaths();
  }
}
