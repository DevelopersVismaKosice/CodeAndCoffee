# Pagination Specification

## Overview

This specification outlines the implementation of pagination functionality for list views across the Short URL application. Pagination will allow users to navigate large datasets efficiently by viewing data in manageable chunks and controlling the number of items displayed per page.

## Business Requirements

1. Users shall be able to navigate through paginated lists of URLs and statistics
2. Users shall be able to select between 3 predefined items-per-page options: 5, 10, and 30
3. Users shall be able to see which page they are currently viewing
4. Users shall be able to navigate to the next and previous pages
5. The system shall preserve the selected pagination settings during the user's session
6. The system shall default to 10 items per page on initial load

## Technical Scope

### Affected Components

#### Server-side (Go)

1. **Repository Layer**
   - `storage/repository.go`: Add pagination support to URL retrieval methods

2. **API Layer**
   - `api/short_handlers.go`: Update handlers to accept and process pagination parameters
   - `api/statistics_handlers.go`: Add pagination support for statistics endpoints
   - `api/types.go`: Define new request/response structures with pagination metadata

3. **Service Layer**
   - `shortener/service.go`: Update service methods to support pagination parameters
   - `statistics/service.go`: Add pagination support for statistics retrieval

#### Client-side (Vue)

1. **Services**
   - `short.service.ts`: Update API methods to include pagination parameters

2. **Views**
   - `DashboardView.vue`: Add pagination UI and logic
   - `StatsView.vue`: Add pagination UI and logic

3. **Components**
   - New `PaginationControls.vue` component for reusability

## Technical Design

### Data Structures

#### Server-side Pagination Parameters

```go
// PaginationParams represents the parameters for pagination
type PaginationParams struct {
    Page     int `json:"page" schema:"page"`
    PageSize int `json:"page_size" schema:"page_size"`
}

// PaginationMeta contains metadata about pagination
type PaginationMeta struct {
    CurrentPage  int   `json:"current_page"`
    PageSize     int   `json:"page_size"`
    TotalItems   int64 `json:"total_items"`
    TotalPages   int   `json:"total_pages"`
    HasNext      bool  `json:"has_next"`
    HasPrevious  bool  `json:"has_previous"`
}
```

#### Client-side Pagination State

```typescript
interface PaginationState {
  currentPage: number;
  pageSize: number;
  totalItems: number;
  totalPages: number;
  hasNext: boolean;
  hasPrevious: boolean;
}

type PageSizeOption = 5 | 10 | 30;
```

### API Endpoints Updates

#### 1. Get User URLs with Pagination

**Endpoint:** `GET /users/{user_id}/urls`

**Query Parameters:**
- `page`: Current page number (default: 1)
- `page_size`: Number of items per page (default: 10, allowed values: 5, 10, 30)

**Response:**

```json
{
  "user_id": 123,
  "urls": [
    {
      "id": 1,
      "hash": "abc123",
      "url": "https://example.com"
    },
    // ...more URLs
  ],
  "pagination": {
    "current_page": 1,
    "page_size": 10,
    "total_items": 42,
    "total_pages": 5,
    "has_next": true,
    "has_previous": false
  }
}
```

#### 2. Get URL Statistics with Pagination

**Endpoint:** `GET /statistics/{user_id}`

**Query Parameters:**
- `page`: Current page number (default: 1)
- `page_size`: Number of items per page (default: 10, allowed values: 5, 10, 30)

**Response:**

```json
{
  "statistics": [
    {
      "url_id": 1,
      "url_hash": "abc123",
      "url": "https://example.com",
      "visits": 42,
      "unique_visits": 10
    },
    // ...more statistics
  ],
  "pagination": {
    "current_page": 1,
    "page_size": 10,
    "total_items": 35,
    "total_pages": 4,
    "has_next": true,
    "has_previous": false
  }
}
```

### Server-side Implementation Details

1. **Update Repository Methods:**

```go
// GetByUserID retrieves URLs for a user with pagination
func (r *Repository) GetByUserID(ctx context.Context, userID int64, page, pageSize int) ([]ShortURL, int64, error) {
    // Calculate offset
    offset := (page - 1) * pageSize
    
    // Get total count first
    var totalCount int64
    countErr := r.db.QueryRowContext(ctx, 
        "SELECT COUNT(*) FROM short_urls WHERE user_id = $1", 
        userID).Scan(&totalCount)
    if countErr != nil {
        return nil, 0, countErr
    }
    
    // Get paginated results
    rows, err := r.db.QueryContext(ctx, 
        "SELECT id, hash, url FROM short_urls WHERE user_id = $1 ORDER BY id DESC LIMIT $2 OFFSET $3", 
        userID, pageSize, offset)
    if err != nil {
        return nil, 0, err
    }
    defer rows.Close()
    
    // Process rows and return results with total count
    // ...
    
    return urls, totalCount, nil
}
```

2. **Update Handler Logic:**

```go
// GetByUserIDHandler handles the retrieval of user's URLs with pagination
func (s *Server) GetByUserIDHandler(w http.ResponseWriter, r *http.Request) {
    // Extract user ID from path
    userID, err := getUserIDFromPath(r)
    if err != nil {
        respondWithError(w, http.StatusBadRequest, "Invalid user ID")
        return
    }
    
    // Parse pagination parameters
    page := 1
    pageSize := 10
    
    if pageParam := r.URL.Query().Get("page"); pageParam != "" {
        if parsedPage, err := strconv.Atoi(pageParam); err == nil && parsedPage > 0 {
            page = parsedPage
        }
    }
    
    if pageSizeParam := r.URL.Query().Get("page_size"); pageSizeParam != "" {
        if parsedPageSize, err := strconv.Atoi(pageSizeParam); err == nil {
            // Validate allowed page sizes
            switch parsedPageSize {
            case 5, 10, 30:
                pageSize = parsedPageSize
            }
        }
    }
    
    // Get URLs with pagination
    urls, totalCount, err := s.repository.GetByUserID(r.Context(), userID, page, pageSize)
    if err != nil {
        respondWithError(w, http.StatusInternalServerError, "Failed to retrieve URLs")
        return
    }
    
    // Calculate pagination metadata
    totalPages := int(math.Ceil(float64(totalCount) / float64(pageSize)))
    hasNext := page < totalPages
    hasPrevious := page > 1
    
    // Create response
    response := GetByUserResponse{
        UserID: userID,
        URLs:   mapToURLResponses(urls),
        Pagination: PaginationMeta{
            CurrentPage:  page,
            PageSize:     pageSize,
            TotalItems:   totalCount,
            TotalPages:   totalPages,
            HasNext:      hasNext,
            HasPrevious:  hasPrevious,
        },
    }
    
    respondWithJSON(w, http.StatusOK, response)
}
```

### Client-side Implementation Details

1. **Update API Service:**

```typescript
// short.service.ts
export class ShortApi {
  // ...existing code...
  
  /**
   * Get URLs for a user with pagination
   */
  async getUserUrls(
    userId: number, 
    page: number = 1, 
    pageSize: number = 10
  ): Promise<GetUserUrlsResponse> {
    try {
      const response = await axios.get<GetUserUrlsResponse>(
        `${this.apiUrl}/users/${userId}/urls`,
        {
          params: { page, page_size: pageSize },
          headers: this.authHeader,
        }
      );
      return response.data;
    } catch (error) {
      this.handleApiError(error);
      throw error;
    }
  }
  
  // Similar updates for statistics endpoints
}
```

2. **Create Pagination Component:**

```vue
<!-- PaginationControls.vue -->
<template>
  <div class="pagination-controls d-flex justify-content-between align-items-center">
    <div class="items-per-page">
      <label for="pageSize">Items per page:</label>
      <select
        id="pageSize"
        v-model="localPageSize"
        class="form-select form-select-sm ms-2"
        @change="onPageSizeChange"
      >
        <option v-for="size in pageSizeOptions" :key="size" :value="size">
          {{ size }}
        </option>
      </select>
    </div>
    
    <div class="page-info">
      Page {{ currentPage }} of {{ totalPages || 1 }}
    </div>
    
    <nav aria-label="Page navigation">
      <ul class="pagination mb-0">
        <li class="page-item" :class="{ disabled: !hasPrevious }">
          <button
            class="page-link"
            @click="onPageChange(currentPage - 1)"
            :disabled="!hasPrevious"
          >
            Previous
          </button>
        </li>
        <li class="page-item" :class="{ disabled: !hasNext }">
          <button
            class="page-link"
            @click="onPageChange(currentPage + 1)"
            :disabled="!hasNext"
          >
            Next
          </button>
        </li>
      </ul>
    </nav>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, watch } from 'vue';

const props = defineProps<{
  currentPage: number;
  pageSize: number;
  totalItems: number;
  totalPages: number;
  hasNext: boolean;
  hasPrevious: boolean;
}>();

const emit = defineEmits<{
  (e: 'page-change', page: number): void;
  (e: 'page-size-change', size: number): void;
}>();

const pageSizeOptions = [5, 10, 30];
const localPageSize = ref(props.pageSize);

const onPageChange = (page: number) => {
  if (page > 0 && page <= props.totalPages) {
    emit('page-change', page);
  }
};

const onPageSizeChange = () => {
  emit('page-size-change', localPageSize.value);
};

// Update local page size when prop changes
watch(() => props.pageSize, (newSize) => {
  localPageSize.value = newSize;
});
</script>
```

3. **Integrate in Views:**

```vue
<!-- DashboardView.vue (partial) -->
<template>
  <div class="dashboard">
    <!-- Table with URL data -->
    <table class="table">
      <!-- Table headers and body -->
    </table>
    
    <!-- Pagination controls -->
    <pagination-controls
      :current-page="pagination.currentPage"
      :page-size="pagination.pageSize"
      :total-items="pagination.totalItems"
      :total-pages="pagination.totalPages"
      :has-next="pagination.hasNext"
      :has-previous="pagination.hasPrevious"
      @page-change="handlePageChange"
      @page-size-change="handlePageSizeChange"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import PaginationControls from '@/components/PaginationControls.vue';
import { ShortApi } from '@/services/short.service';

const shortApi = new ShortApi();
const route = useRoute();
const urls = ref<Array<UserShortURLResponse>>([]);

const pagination = reactive<PaginationState>({
  currentPage: 1,
  pageSize: 10,
  totalItems: 0,
  totalPages: 0,
  hasNext: false,
  hasPrevious: false
});

// Load data with current pagination state
const loadData = async () => {
  try {
    const userId = 123; // Get from auth context or route
    const response = await shortApi.getUserUrls(
      userId,
      pagination.currentPage,
      pagination.pageSize
    );
    
    urls.value = response.urls;
    
    // Update pagination state from response
    pagination.totalItems = response.pagination.total_items;
    pagination.totalPages = response.pagination.total_pages;
    pagination.hasNext = response.pagination.has_next;
    pagination.hasPrevious = response.pagination.has_previous;
  } catch (error) {
    console.error('Failed to load URLs', error);
  }
};

// Event handlers
const handlePageChange = (page: number) => {
  pagination.currentPage = page;
  loadData();
};

const handlePageSizeChange = (size: number) => {
  pagination.pageSize = size;
  pagination.currentPage = 1; // Reset to first page on size change
  loadData();
  
  // Store preference in localStorage
  localStorage.setItem('preferredPageSize', size.toString());
};

onMounted(() => {
  // Restore user preference if available
  const storedPageSize = localStorage.getItem('preferredPageSize');
  if (storedPageSize) {
    const size = parseInt(storedPageSize);
    if ([5, 10, 30].includes(size)) {
      pagination.pageSize = size;
    }
  }
  
  loadData();
});
</script>
```

## User Interface Design

### Pagination Controls

The pagination controls will be positioned at the bottom of data tables and will include:

1. **Page Size Selector**
   - Dropdown with options: 5, 10, 30
   - Located on the left side

2. **Page Information**
   - Text display: "Page X of Y"
   - Located in the center

3. **Navigation Buttons**
   - "Previous" and "Next" buttons
   - Located on the right side
   - Disabled state when at first/last page

### Wireframe

```
┌───────────────────────────────────────────────────────────────┐
│                                                               │
│  ┌─────────┬───────────────┬─────────────┬─────────────────┐  │
│  │ Hash    │ Original URL  │ Created     │ Actions         │  │
│  ├─────────┼───────────────┼─────────────┼─────────────────┤  │
│  │ abc123  │ example.com   │ 2023-01-01  │ Edit  Delete    │  │
│  ├─────────┼───────────────┼─────────────┼─────────────────┤  │
│  │ def456  │ example.org   │ 2023-01-02  │ Edit  Delete    │  │
│  ├─────────┼───────────────┼─────────────┼─────────────────┤  │
│  │ ...     │ ...           │ ...         │ ...             │  │
│  └─────────┴───────────────┴─────────────┴─────────────────┘  │
│                                                               │
│  ┌────────────────┐     ┌───────────────┐    ┌─────────────┐  │
│  │ Items per page:│     │ Page 1 of 5   │    │ Previous Next│  │
│  │ [10      ▼]    │     └───────────────┘    └─────────────┘  │
│  └────────────────┘                                           │
│                                                               │
└───────────────────────────────────────────────────────────────┘
```

## Performance Considerations

1. **Database Performance**
   - Add indexes on frequently filtered columns to optimize pagination queries
   - Consider adding a composite index on (user_id, created_at) to optimize the common query pattern

2. **UI Performance**
   - Implement loading indicators during page transitions
   - Consider implementing debouncing for pagination control interactions to prevent rapid successive API calls

3. **Caching**
   - Consider implementing client-side caching of previously loaded pages to reduce API calls
   - Add appropriate cache headers to API responses

## Testing Strategy

1. **Unit Tests**
   - Test pagination calculations in repository layer
   - Test pagination parameter parsing in handler layer
   - Test UI component behavior with different pagination states

2. **Integration Tests**
   - Test API endpoints with different pagination parameters
   - Verify response structure and pagination metadata
   - Test edge cases (empty results, last page, etc.)

3. **End-to-End Tests**
   - Test user flows through paginated data
   - Verify preservation of pagination settings across sessions

## Implementation Plan

1. **Phase 1: Backend Implementation**
   - Update repository methods with pagination support
   - Modify API handlers to accept pagination parameters
   - Update response structures with pagination metadata
   - Add unit tests for pagination logic

2. **Phase 2: Frontend Implementation**
   - Create reusable pagination component
   - Update API service methods to support pagination
   - Implement pagination UI in DashboardView
   - Implement pagination UI in StatsView
   - Add client-side tests

3. **Phase 3: Testing and Refinement**
   - End-to-end testing of pagination flows
   - Performance optimization
   - UX refinements based on testing feedback

## Success Criteria

1. Users can navigate through paginated data with Next/Previous controls
2. Users can adjust the number of items per page (5, 10, 30)
3. System preserves pagination preferences across sessions
4. All pagination operations complete within acceptable performance thresholds (< 500ms)
5. All tests pass successfully

## Future Enhancements

1. **Advanced Pagination Features**
   - Jump to specific page
   - First/Last page navigation buttons
   - Keyboard navigation support

2. **UI Enhancements**
   - Sorting capabilities integrated with pagination
   - Filtering capabilities integrated with pagination
   - Infinite scroll as an alternative pagination mode