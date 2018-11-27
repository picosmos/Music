import { Injectable } from "@angular/core";
import { LibraryService, MusicMetaDataModel } from "./LibraryService";
import * as mm from "musicmetadata";

let fs/*: typeof Fs*/ = window['require']('fs');
let path = window['require']('path');

const promisify = f => (...args) => new Promise((a, b) => f(...args, (err, res) => err ? b(err) : a(res)));

@Injectable()
export class IndexingService {
    constructor(
        private _libraryService: LibraryService
    ) {
    }

    public async reCreateIndex() {
        let files = await this.findFiles();
        await this._libraryService.deleteAllMusic();
        for (let file of files) {
            console.log({file});
            try {
                let data = await this.getMetaData(file);
                let newData = this.convertMetaDataToModel(data, file);
                await this._libraryService.addMusic(newData);
            }
            catch (ex) {
                console.log({ "file": file, ex: ex });
            }
        }
    }

    public convertMetaDataToModel(data: MM.Metadata, file: string) {
        let result = new MusicMetaDataModel();
        result.artists = data.artist;
        result.album = data.album;
        result.albumArtists = data.albumartist;
        result.title = data.title;
        result.year = data.year;
        result.track = data.track;
        result.disk = data.track;
        result.genres = data.genre;
        result.picture = data.picture;
        result.duration = data.duration;
        result.file = file;
        return result;
    }

    public async findFiles(): Promise<any[]> {
        let paths = await this._libraryService.getPaths();
        let result = [];
        for (let p of paths) {
            let files = <string[]>await promisify(fs.readdir)(p);
            for (let file of files) {
                result.push(path.join(p, file));
            }
        }
        return result;
    }

    public async getMetaData(path: string): Promise<MM.Metadata> {
        return new Promise<MM.Metadata>((resolve, reject) => {
            var stream = fs.createReadStream(path);
            mm(stream, (err, metadata: MM.Metadata) => {
                if (err) {
                    reject();
                }
                else {
                    resolve(metadata);
                }
                stream.close();
            });
        });
    }
}