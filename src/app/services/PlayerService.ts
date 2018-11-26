import { Injectable, EventEmitter } from "@angular/core";
import { MusicViewModel } from "../models/MusicViewModel";
import { Howl } from "howler"

@Injectable()
export class PlayerService {

    constructor() {

    }

    public currentItemChanged = new EventEmitter<MusicViewModel>();


    private files: MusicViewModel[] = [];
    public currentIndex: number;

    public setFiles(files: MusicViewModel[]) {
        this.files = files;
        if (this.files.length > 0) {
            this.currentItemChanged.emit(this.files[0]);
        }
    }

    public play(): void {
        var sound = new Howl(<IHowlProperties>{});
        sound.play();
    }

    public pause(): void {

    }

    // public getCurrentArtist() {
    //     return "Artist";//ToDo
    // }

    // public getCurrentTitle() {
    //     return "Title";//ToDo
    // }

    // public getCurrentFileName() {
    //     if (this.currentIndex) {
    //         let item = this.files[this.currentIndex];
    //         if (item) {
    //             return item.path;//ToDo
    //         }
    //     }
    //     return null;
    // }
}