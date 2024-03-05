#include <unistd.h>
#include <fcntl.h>
#include <assert.h>
#include <stdio.h>

int main() {
    int fd = open("tmp", O_CREAT | O_RDWR, 0666);
    assert(fd > 0);
    int ret = write(fd, "world", 5);
    assert(ret == 5);
    ret = close(fd);
    assert(ret == 0);

    // Introduce a sync point
    sync();

    ret = rename("tmp", "file1");
    assert(ret == 0);
    
        // Introduce another sync point
    sync();
    
    printf("Updated\n");
}