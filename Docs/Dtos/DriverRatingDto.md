# DriverRatingDto Documentation

## Class: `DriverRatingDto`

<details>
<summary><strong>Purpose</strong></summary>
Represents driver rating data, including their average rating, the number of ratings, and related details. This DTO acts as a transfer object between layers.
</details>

---

### Properties

<details>
<summary><strong>DriverId</strong></summary>
- **Type**: `int`
- **Description**: The unique identifier for the driver.
</details>

<details>
<summary><strong>AverageRating</strong></summary>
- **Type**: `double`
- **Description**: The average rating for the driver, calculated from all user ratings.
</details>

<details>
<summary><strong>RatingCount</strong></summary>
- **Type**: `int`
- **Description**: The total number of ratings received by the driver.
</details>

<details>
<summary><strong>Comment</strong></summary>
- **Type**: `string`
- **Description**: Optional comment or feedback about the driver.
</details>

---

### Example Usage:

```csharp
var driverRating = new DriverRatingDto
{
    DriverId = 1001,
    AverageRating = 4.75,
    RatingCount = 20,
    Comment = "Very professional and polite driver."
};

Console.WriteLine(driverRating);
