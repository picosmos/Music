import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";
import { MusicViewModel } from "../models/MusicViewModel";

@Component({
  selector: "music-list",
  templateUrl: "./MusicList.web.html",
  styleUrls: ["./MusicList.web.less"]
})
export class MusicListComponent {
  constructor(
  ) {
  }

  @Input()
  public items: MusicViewModel[] = [];

  @Output()
  itemClicked: EventEmitter<MusicViewModel> = new EventEmitter();

  public onItemClick(item: MusicViewModel) {
    this.itemClicked.emit(item);
  }

  public isDragging: boolean = false;

  public onDragOver($event) {
    $event.preventDefault();
    this.isDragging = true;
  }

  public onDrop($event) {
    $event.preventDefault();

    if ($event.dataTransfer.items) {
      // Use DataTransferItemList interface to access the file(s)
      for (var i = 0; i < $event.dataTransfer.items.length; i++) {
        // If dropped items aren't files, reject them
        if ($event.dataTransfer.items[i].kind === 'file') {
          var file = $event.dataTransfer.items[i].getAsFile();
          var item = MusicViewModel.fromFile(file);
          this.items.push(item)
        }
      }
    }
    else {
      // Use DataTransfer interface to access the file(s)
      for (var i = 0; i < $event.dataTransfer.files.length; i++) {
        var item = MusicViewModel.fromFile($event.dataTransfer.files[i]);
        this.items.push(item)
      }
    }
  }
}
