package com.visma.codecoffee.memcached;


import net.spy.memcached.MemcachedClient;
import net.spy.memcached.internal.OperationFuture;
import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.Test;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.IOException;
import java.net.InetSocketAddress;
import java.net.SocketAddress;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.Map;
import java.util.Optional;
import java.util.concurrent.ExecutionException;

import static org.junit.jupiter.api.Assertions.*;

public class MemcachedExamplesTest {

    private static final Logger log = LoggerFactory.getLogger(MemcachedExamplesTest.class);

    public static final String HOSTNAME = "127.0.0.1";
    public static final int PORT = 11211;

    public static final String KEY = "MY_KEY";
    public static final int TTL = 60;

    @Test
    void shouldAddItemToCache_whenNoItemExist() throws Exception {
        // arrange
        MemcachedClient client = getMemcachedClient();
        // act
        OperationFuture<Boolean> result = client.add(KEY, TTL, "my value");
        // assert
        assertTrue(result.get(), "add operation result should be true");

        printKey(client);
    }

    @Test
    void shouldNotAddItemToCache_whenItemAlreadyExist() throws Exception {
        // arrange
        MemcachedClient client = getMemcachedClient();
        client.set(KEY, TTL, "my value");
        // act
        OperationFuture<Boolean> result = client.add(KEY, TTL, "new value");
        // assert
        assertFalse(result.get(), "add operation result should be false");

        printKey(client);
    }

    @Test
    void shouldAppendItemInCache_whenItemAlreadyExists() throws IOException {
        // arrange
        var client = getMemcachedClient();
        client.add(KEY, TTL, "value");
        client.append(KEY, "-append");
        // act
        var result = client.get(KEY);
        // assert
        assertEquals("value-append", result);

        printKey(client);
    }

    @Test
    void shouldNotAddItemToCache_whenItemIsBiggerThan1MB() throws Exception {
        // arrange
        var client = getMemcachedClient();
        var value = readTooBigFile();
        // act
        var result = client.set(KEY, TTL, value);
        // assert
        assertThrows(ExecutionException.class, result::get);

        printKey(client);
    }

    @Test
    void shouldGetItemFromStore_whenItemIsNotInCache() throws Exception {
        // arrange
        var client = getMemcachedClient();
        // act
        var result = Optional.ofNullable(client.get(KEY))
                .orElseGet(() -> {
                    var value = "value from store";
                    client.set(KEY, TTL, value);
                    return value;
                });
        // assert
        assertEquals("value from store", result);

        printKey(client);
    }

    @Test
    void shouldShowStats() throws IOException {
        // arrange
        var client = getMemcachedClient();
        // act
        var result = client.getStats();
        // assert
        assertNotNull(result);

        printStats(result);
    }

    @AfterEach
    void clearMemcache() throws IOException {
        log.info("-- clearing cache ---");
        var client = getMemcachedClient();
        client.flush();
    }


    private MemcachedClient getMemcachedClient() throws IOException {
        var address = new InetSocketAddress(HOSTNAME, PORT);
        return new MemcachedClient(address);
    }

    private void printKey(MemcachedClient client) {
        log.info("MY_KEY value: " + client.get(KEY));
    }

    private void printStats(Map<SocketAddress, Map<String, String>> stats) {
        stats.values().stream().forEach(
                stat -> stat.forEach((key, value) -> log.info("{}: {}", key, value))
        );
    }

    private byte[] readTooBigFile() throws IOException {
        Path path = Paths.get("src/test/resources/bigFile.txt");
        return Files.readAllBytes(path);
    }
}
