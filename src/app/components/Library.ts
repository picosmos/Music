import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";
import { MusicViewModel } from "../models/MusicViewModel";

@Component({
  selector: "library",
  templateUrl: "./Library.web.html",
  styleUrls: ["./Library.web.less"]
})
export class LibraryComponent {
  constructor(
  ) {
  }

  public paths: string[] = [];

  public addPath(path: string) {
    this.paths.push(path);
  }
}
