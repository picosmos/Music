import { Injectable } from "@angular/core";
import { LibraryService } from "./LibraryService";
//import * as Fs from 'fs';

let fs/*: typeof Fs*/ = window['require']('fs');

const promisify = f => (...args) => new Promise((a, b) => f(...args, (err, res) => err ? b(err) : a(res)));

@Injectable()
export class IndexingService {
    constructor(
        private _libraryService: LibraryService
    ) {
    }

    public async findFiles(): Promise<any[]> {
        let paths = await this._libraryService.getPaths();
        let result = [];
        for (let path of paths) {
            let files = await promisify(fs.readdir)(path);
            result = result.concat(files);
        }
        return result;
    }
}