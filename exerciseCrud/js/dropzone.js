

// 파일 드롭 다운
function fileDropDown() {
    var dropZone = $("#dropZone");
    //Drag기능 
    dropZone.on('dragenter', function (e) {
        e.stopPropagation();
        e.preventDefault();
        // 드롭다운 영역 css
        dropZone.css('background-color', '#E3F2FC');
    });
    dropZone.on('dragleave', function (e) {
        e.stopPropagation();
        e.preventDefault();
        // 드롭다운 영역 css
        dropZone.css('background-color', '#fdfaf8');
    });
    dropZone.on('dragover', function (e) {
        e.stopPropagation();
        e.preventDefault();
        // 드롭다운 영역 css
        dropZone.css('background-color', '#E3F2FC');
    });
    dropZone.on('drop', function (e) {
        e.preventDefault();
        // 드롭다운 영역 css
        dropZone.css('background-color', '#fdfaf8');

        var files = e.originalEvent.dataTransfer.files;
        if (files != null) {
            if (files.length < 1) {
                /* alert("폴더 업로드 불가"); */
                console.log("폴더 업로드 불가");
                return;
            } else {
                selectFile(files)
            }
        } else {
            alert("ERROR");
        }
    });
}
// 파일 바인딩시 작동 함수
function selectFile(fileObject) {
    var files = null;
    files = fileObject;

    // 다중파일 등록
    if (files != null) {
        console.log(files);
        for (var i = 0; i < files.length; i++) {
            if ((totalFileCount + 1) > maxUploadCount) {
                alert("업로드 허용 파일 갯수를 초과하였습니다.(" + maxUploadCount + "개)");
                break;
            }
            // 파일 이름
            var fileName = files[i].name;
            
            var fileNameArr = fileName.split("\.");
            // 확장자
            var ext = fileNameArr[fileNameArr.length - 1];

            var fileSize = files[i].size; // 파일 사이즈(단위 :byte)
            console.log("fileSize=" + fileSize);
            if (fileSize <= 0) {
                console.log("0kb file return");
                return;
            }

            var fileSizeKb = fileSize / 1024; // 파일 사이즈(단위 :kb)
            var fileSizeMb = fileSizeKb / 1024;    // 파일 사이즈(단위 :Mb)

            var fileSizeStr = "";
            console.log("fileSizeKb=" + parseInt(fileSizeKb));
            fileSizeStr = parseInt(fileSizeKb) + " kb";

             // 파일 사이즈 검사
            if (fileSizeKb > uploadSize) {             
                alert("파일 용량이 초과되었습니다.(" + Math.ceil(fileSizeKb) + "KB / " + uploadSize + "KB)");
                break;

            } else {
                // 전체 파일 사이즈
                totalFileSize += fileSizeMb;
                // 전체 파일 카운트
                totalFileCount += 1;

                // 파일 배열에 넣기
                fileList[fileIndex] = files[i];

                // 파일 사이즈 배열에 넣기
                fileSizeList[fileIndex] = fileSizeMb;

                // 업로드 파일 목록 생성
                addFileList(fileIndex, fileName, fileSizeStr);

                // 파일 번호 증가
                fileIndex++;
            }
        }

        if (totalFileCount > 0) {
            $("#fileListTable").show();
        } else {
            $("#fileListTable").hide();
        }
    } else {
        alert("ERROR");
    }
}
// 드랍시 동적 업로드 파일 목록 생성
function addFileList(fIndex, fileName, fileSizeStr) {

    var html = "";
    html += "<tr id='fileTr_" + fIndex + "'>";
    html += "    <td class='txt_left'><button type='button' class='btn btn_delete' onclick='deleteFile(" + fIndex + "); return false;'>삭제</button></td>";
    html += "    <td>" + fileName + "</td>";
    html += "    <td>" + fileSizeStr + "</td>";
    html += "</tr>";

    $('#fileTableTbody').append(html);
}


// arguments[0] = fIndex 
// arguments[1] = fileId
// 업로드 파일 삭제
function deleteFile() {
        

        console.log("deleteFile.fIndex=" + arguments[0]);
        // 전체 파일 사이즈 수정
        totalFileSize -= fileSizeList[arguments[0]];
        // 전체 파일 카운트
        totalFileCount -= 1;

        // 파일 배열에서 삭제
        delete fileList[arguments[0]];

        // 파일 사이즈 배열 삭제
        delete fileSizeList[arguments[0]];

        // 업로드 파일 테이블 목록에서 삭제
        $("#fileTr_" + arguments[0]).remove();
        
        if (totalFileCount > 0) {
            $("#fileListTable").show();
        } else {
            $("#fileListTable").hide();
        }

        //변수2개 일때
    if (arguments.length > 1) {
            //원본파일 삭제 리스트에 추가
            dfileList.push(arguments[1]);        
        }

    
};