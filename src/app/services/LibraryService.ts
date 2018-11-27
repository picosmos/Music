import { Injectable } from "@angular/core";
import * as Datastore from "nedb";

@Injectable()
export class LibraryService {
    constructor() {
        this._db = new Datastore({ filename: 'datafile.nedb', autoload: true });
    }

    private _db: Datastore;

    public addPath(path: string): Promise<void> {
        return new Promise((resolve, reject) => {
            try {
                this._db.insert(new PathModel(path), (err, document) => {
                    if (err) {
                        reject(err);
                    }
                    else {
                        resolve();
                    }
                });
            }
            catch (ex) {
                reject(ex);
            }
        });
    }

    public async deletePath(path: string): Promise<number> {
        return new Promise<number>((resolve, reject) => {
            try {
                this._db.remove(new PathModel(path), (err, n) => {
                    if (err) {
                        reject(err);
                    }
                    else {
                        resolve(n);
                    }
                });
            }
            catch (ex) {
                reject(ex);
            }
        });
    }

    public async deleteAllMusic(): Promise<number> {
        return new Promise<number>((resolve, reject) => {
            try {
                this._db.remove({ type: "music" }, { multi: true }, (err, n) => {
                    if (err) {
                        reject(err);
                    }
                    else {
                        resolve(n);
                    }
                });
            }
            catch (ex) {
                reject(ex);
            }
        });
    }

    public async getPaths(): Promise<string[]> {
        return new Promise<string[]>((resolve, reject) => {
            try {
                this._db.find({ type: "path" }, (err, docs: PathModel[]) => {
                    if (err) {
                        reject(err);
                        return;
                    }
                    let result = [];
                    for (let path of docs) {
                        result.push(path.path);
                    }
                    resolve(result);
                });
            }
            catch (ex) {
                reject(ex);
            }
        });
    }


    public async getMusic(): Promise<MusicMetaDataModel[]> {
        return new Promise<MusicMetaDataModel[]>((resolve, reject) => {
            try {
                this._db.find({ type: "music" }, (err, docs: MusicMetaDataModel[]) => {
                    if (err) {
                        reject(err);
                        return;
                    }
                    resolve(docs);
                });
            }
            catch (ex) {
                reject(ex);
            }
        });
    }

    public addMusic(data: MusicMetaDataModel): Promise<void> {
        return new Promise((resolve, reject) => {
            try {
                this._db.insert(data, (err, document) => {
                    if (err) {
                        reject(err);
                    }
                    else {
                        resolve();
                    }
                });
            }
            catch (ex) {
                reject(ex);
            }
        });
    }
}

export interface IModel {
    type: string;
}

export class PathModel implements IModel {
    public type = "path";

    public constructor(
        public path: string
    ) {
    }
}

export class MusicMetaDataModel implements IModel {
    public type = "music";

    public artists: string[];
    public album: string;
    public albumArtists: string[];
    public title: string;
    public year: string;
    public track: { no: number, of: number } | null;
    public disk: { no: number, of: number } | null;
    public genres: string[];
    public picture: any;
    public duration: number;

    public file: string;
}